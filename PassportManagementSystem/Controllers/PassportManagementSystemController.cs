using PassportManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace PassportManagementSystem.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]//Prevents Page from going back when back button is pressed
    public class PassportManagementSystemController : Controller
    {
        //Displays Home View
        public ActionResult Home()
        {
            return View();
        }
        //Displays About View
        public ActionResult About()
        {
            return View();
        }
        //Displays Contact View
        public ActionResult Contact()
        {
            return View();
        }
        //Displays Register View
        public ActionResult Register()
        {
            return View();
        }
        //When user submits the form in register page it validates
        //If validation is successfull then it goes to DBOperations Class and
        //fetches the data and store in ViewBag.data(used in .cshtml to display) and empty the fiels in the form
        //Else it returns to the same view with validation messages
        [HttpPost]
        public ActionResult Register(UserRegistration R)
        {
            R.ApplyType = "Passport User";
            ModelState.Remove("UserID");
            ModelState.Remove("ApplyType");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                UserRegistration details = DBOperations.Registration(R);
                ViewBag.data = details;
                ModelState.Clear();
                return View();
            }
            else
                return View();
        }
        //when user logins if user type is passport then it redirects to this Action
        //DBOperations fetches state data on page load to view inorder to select state by the user
        //Used Session to restrict users to directly access the link without login
        public ActionResult ApplyPassport()
        {
            if (Session["EmailAddress"] == null && Session["ApplyType"] == null && Session["UserID"] == null)
                return RedirectToAction("Login");
            ViewBag.UserID = Session["UserID"];
            return View();
        }
        //When user apply the passport it validates
        //If validation is successfull then it goes to DBOperations Class and
        //fetches the data and sends data or error messages to view
        [HttpPost]
        public ActionResult ApplyPassport([Bind(Exclude = "ProofOfCitizenship, Photo, BirthCertificate")] PassportApplication P, HttpPostedFileBase ProofOfCitizenship, HttpPostedFileBase Photo, HttpPostedFileBase BirthCertificate)
        {
            ModelState.Remove("ReasonForReIssue");
            ModelState.Remove("PassportNumber");
            ModelState.Remove("ProofOfCitizenship");
            ModelState.Remove("Photo");
            ModelState.Remove("BirthCertificate");
            if (ModelState.IsValid)
            {
                ViewBag.UserID = Session["UserID"];
                if (ProofOfCitizenship != null)
                {
                    int filelength = ProofOfCitizenship.ContentLength;
                    byte[] Myfile_1 = new byte[filelength];
                    ProofOfCitizenship.InputStream.Read(Myfile_1, 0, filelength);
                    P.ProofOfCitizenship = Myfile_1;
                }
                else
                {
                    ViewBag.error = "Invalid File Format, Upload only PDF files";
                }

                if (Photo != null)
                {
                    int filelength = Photo.ContentLength;
                    byte[] Myfile_2 = new byte[filelength];
                    Photo.InputStream.Read(Myfile_2, 0, filelength);
                    P.Photo = Myfile_2;
                }
                else
                {
                    ViewBag.error = "Invalid File Format, Upload only JPEG files";
                }

                if (BirthCertificate != null)
                {
                    int filelength = BirthCertificate.ContentLength;
                    byte[] Myfile_3 = new byte[filelength];
                    BirthCertificate.InputStream.Read(Myfile_3, 0, filelength);
                    P.BirthCertificate = Myfile_3;
                }
                else
                {
                    ViewBag.error = "Invalid File Format, Upload only PDF files";
                }

                PassportApplication details = DBOperations.ApplyPassport(P);
                if (details != null)
                    ViewBag.data = details;
                else
                    ViewBag.error = "Passport Number w.r.t UserID already generated";
                ModelState.Clear();
                return View();
            }
            else
            {
                ViewBag.UserID = Session["UserID"];
                return View();
            }
        }
    }
}
