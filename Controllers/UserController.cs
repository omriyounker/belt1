using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using belt1.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace belt1.Controllers
{
    public class UserController : Controller
    {
        private BeltContext _context;

        public UserController(BeltContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password)
        {   
            User account = _context.users.SingleOrDefault(x => (x.Email == Email));
            if(account.Password == Password){
                HttpContext.Session.SetInt32("userid", account.UserId);
                return RedirectToAction("Dashboard", "Event");
            }
            ViewBag.Error = "Email or Password is incorrect";
            return View("Index");
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(User user, string Password2){
            Console.WriteLine("GOT INTO REGISTER!");
            string pass = user.Password;
            bool rule1 = pass.Any(char.IsLetter);
            bool rule2 = pass.Any(char.IsNumber);
            bool rule3 = pass.Any(char.IsSymbol);
            Console.WriteLine("!*!*!*!*!*!*!**!*!*!*");
            Console.WriteLine(rule1 +  " " + rule2 + " " + rule3);

            if(rule1 && rule2 && rule3){
                User account = _context.users.SingleOrDefault(x => (x.Email == user.Email));
                if (account != null){
                    ViewBag.Error = "Account already exists for that email account";
                    return View("Index");
                }
                if(user.Password != Password2){
                    ViewBag.Error = "Password and Password Verification must match";
                    return View("Index");
                }
                if(ModelState.IsValid){
                    _context.users.Add(user);
                    
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("userid", user.UserId);
                    return RedirectToAction("Dashboard", "Event");
                }
                return View("Index");
            }
            ViewBag.Error = "Password must have at least 1 letter, 1 number, and one special character";
            return View("Index");
        }
    }
}
