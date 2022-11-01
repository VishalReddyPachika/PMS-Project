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
    }
}