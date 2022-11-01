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
    }
}