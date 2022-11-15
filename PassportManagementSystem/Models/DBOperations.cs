using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PassportManagementSystem.Models
{
    public class DBOperations
    {
        static PassportManagementSystemEntities P = new PassportManagementSystemEntities();

        //This method generates UserID,Password and CitizenType based on validations and conditions mentioned in the SRD and insert in database
        public static UserRegistration Registration(UserRegistration R)
        {
            try
            {
                string citizentype = string.Empty;
                string userid = string.Empty;

                if (R.ApplyType == "Passport User")
                {
                    int passid = (from c in P.UserRegistrations
                                  where c.ApplyType == R.ApplyType
                                  select c).Count() + 1;
                    userid = R.ApplyType.Substring(0, 4).ToUpper() + "-" + string.Format("{0:0000}", passid);
                }
                else if (R.ApplyType == "Visa")
                {
                    int visaid = (from c in P.UserRegistrations
                                  where c.ApplyType == R.ApplyType
                                  select c).Count() + 1;
                    userid = R.ApplyType.Substring(0, 4).ToUpper() + "-" + string.Format("{0:0000}", visaid);
                }

                //CitizenType
                int age = (int)(DateTime.Today.Subtract(R.DateOfBirth).TotalDays / 365);
                if (age >= 0 && age < 1)
                    citizentype = "Infant";
                else if (age >= 1 && age < 10)
                    citizentype = "Children";
                else if (age >= 10 && age < 20)
                    citizentype = "Teen";
                else if (age >= 20 && age < 50)
                    citizentype = "Adult";
                else if (age >= 50)
                    citizentype = "Senior Citizen";

                //Inserting into Database
                R.UserID = userid;
                R.CitizenType = citizentype;
                P.UserRegistrations.Add(R);
                P.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return R;
        }
        //This method checks seperately for 'Passport' whether the EmailId is already registered
        public static bool EmailValidation(UserRegistration R)
        {
            try
            {
                List<string> emailIDs = (from u in P.UserRegistrations
                                         where u.ApplyType == R.ApplyType
                                         select u.EmailAddress).ToList();
                if (emailIDs.Contains(R.EmailAddress))
                    return true;
                else
                    return false;
            }
            catch (Exception) { }
            return false;
        }
        //This method generates PassportNumber,RegistrationCost and ExpiryDate based on validations and conditions mentioned in the SRD and insert in database
        public static PassportApplication ApplyPassport(PassportApplication PA)
        {
            try
            {
                string passportid = string.Empty;
                //Calculates ExpiryDate based on IssueDate
                DateTime expiryDate = PA.IssueDate.AddYears(10);

                //Generates PassportNumber
                if (PA.BookletType == "30 Pages")
                {
                    var fps30 = (from c in P.PassportApplications
                                 select c.PassportNumber.Substring(c.PassportNumber.Length - 4, c.PassportNumber.Length)).Max();
                    if (fps30 == null)
                        fps30 = "0";
                    passportid = "FPS-30" + string.Format("{0:0000}", int.Parse(fps30) + 1);
                }
                else if (PA.BookletType == "60 Pages")
                {
                    var fps60 = (from c in P.PassportApplications
                                 select c.PassportNumber.Substring(c.PassportNumber.Length - 4, c.PassportNumber.Length)).Max();
                    if (fps60 == null)
                        fps60 = "0";
                    passportid = "FPS-60" + string.Format("{0:0000}", int.Parse(fps60) + 1);
                }

                //Calculates RegistrationCost based on Type of Service
                int registrationcost = 0;
                if (PA.TypeOfService == "Regular")
                    registrationcost = 2500;
                else
                    registrationcost = 5000;

                //Inserting into database
                PA.PassportNumber = passportid;
                PA.ExpiryDate = expiryDate;
                PA.Amount = registrationcost;
                PA.ReasonForReIssue = "N/A";
                PA.Status = "Initiated";

                int usercount = (from c in P.PassportApplications
                                 where c.UserID == PA.UserID
                                 select c).Count();
                if (usercount == 0)//Checks whether the user already registered or not
                {
                    P.PassportApplications.Add(PA);
                    P.SaveChanges();
                }
                else
                    return null;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return PA;
        }
        public static ApplicantDetails Applicant(String userID)
        {
            try
            {
                List<UserRegistration> users = P.UserRegistrations.ToList();
                List<PassportApplication> passports = P.PassportApplications.ToList();

                ApplicantDetails applicantDetails = (from c in passports
                                                     join r in users on c.UserID equals r.UserID
                                                     where r.UserID == userID
                                                     select new ApplicantDetails
                                                     {
                                                         passports = c,
                                                         users = r
                                                     }).FirstOrDefault();

                return applicantDetails;
            }
            catch (Exception) { }
            return null;
        }
        //Forgot password
        public static string ForgotPassword(UserRegistration U)
        {
            try
            {
                UserRegistration UR = (from c in P.UserRegistrations
                                       where c.EmailAddress == U.EmailAddress && c.SecurityQuestion == U.SecurityQuestion && c.SecurityAnswer == U.SecurityAnswer
                                       select c).FirstOrDefault();
                if (UR != null)
                    return "Success";
                else
                    return "Your security question and answer doesn't match";
            }
            catch (Exception) { }
            return null;
        }
        //Reset Password
        public static string ResetPassword(UserRegistration U)
        {
            try
            {
                UserRegistration UR = (from c in P.UserRegistrations
                                       where c.EmailAddress == U.EmailAddress
                                       select c).FirstOrDefault();
                UR.Password = U.Password;
                UR.ConfirmPassword = U.Password;
                P.Configuration.ValidateOnSaveEnabled = false;
                P.SaveChanges();
                if (UR != null)
                    return "Success";
                else
                    return "Password Update UnSuccessfull";
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
