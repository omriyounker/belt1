using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using belt1.Models;
using System.Linq;

namespace belt1.Controllers
{
    public class EventController : Controller
    {
        private BeltContext _context;

        public EventController(BeltContext context)
        {
            _context = context;
        }
        public void IsLoggedIn(){
            int? uid = HttpContext.Session.GetInt32("userid");
            List<User> users = _context.users.ToList();
            User account = users.SingleOrDefault(x => (x.UserId == uid));
            if(unchecked( uid != (int)uid )){
                RedirectToAction("Dashboard");
            }
        }
        public bool CheckConflict(Event input){
            bool check = false;
            DateTime endTime = new DateTime();
            DateTime endTimeinput = new DateTime();
            if(input.HM == "Hours"){
                endTimeinput = input.Date.AddHours(input.Duration);
            }
            else if(input.HM == "Minutes"){
                endTimeinput = input.Date.AddMinutes(input.Duration);
            }
            else if(input.HM == "Days"){
                endTimeinput = input.Date.AddDays(input.Duration);
            }
            int? uid = HttpContext.Session.GetInt32("userid");
            List<User> users = _context.users.Include(one => one.Going).ThenInclude(each => each.Event).ToList();
            User account = users.SingleOrDefault(x => (x.UserId == uid));
            foreach(Rsvp ev in account.Going){
                if(ev.Event.HM == "Hours"){
                    endTime = ev.Event.Date.AddHours(ev.Event.Duration);
                }
                else if(ev.Event.HM == "Minutes"){
                    endTime = ev.Event.Date.AddMinutes(ev.Event.Duration);
                }
                else{
                    endTime = ev.Event.Date.AddDays(ev.Event.Duration);
                }//generate the end time

                if(DateTime.Compare(endTimeinput, ev.Event.Date) < 0){
                    check = true;
                }else if(DateTime.Compare(input.Date, endTime)> 0){
                    check = true;
                }else{return false;}

    
            }
            return true;

        }

        // GET: /Home/
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            IsLoggedIn();
            int? uid = HttpContext.Session.GetInt32("userid");
            List<User> users = _context.users.Include(uno => uno.Going).ToList();
            User account = users.SingleOrDefault(x => (x.UserId == uid));
            List<Event> totallist = _context.events.Include(one => one.Coming).ToList();
            totallist.OrderByDescending(one => one.Date);
            List<Event> passlist = new List<Event>();
            foreach(Event item in totallist){
                if(DateTime.Compare(item.Date, DateTime.Now) >= 0){
                    passlist.Add(item);
                }
            }
            
            ViewBag.userinfo = account;
            ViewBag.list = passlist;
            ViewBag.uid = (int)uid;
            return View();
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("userid");
            return RedirectToAction("Index", "User");
        }
        [HttpGet]
        [Route("new")]
        public IActionResult New()
        {
            IsLoggedIn();
            return View();
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(Event input, string hm)
        {

            DateTime test = DateTime.Now;
            IsLoggedIn();
            TimeSpan time = TimeSpan.Parse(input.Time.ToString());
            input.Time = string.Format("{0:hh:mm tt}", input.Time);
            int? uid = HttpContext.Session.GetInt32("userid");
            List<User> users = _context.users.ToList();
            User account = users.SingleOrDefault(x => (x.UserId == uid));
            input.Date = input.Date.Add(time);
            input.Creator = account.UserId;
            input.CreatorName = account.FirstName;
            if(DateTime.Compare(input.Date, test) < 0){
                ViewBag.Error = "Date must be after now";
                return View("New");
            }
            _context.events.Add(input);
            _context.SaveChanges();

            return RedirectToAction("Event", new{num=input.EventId});
        }
        [HttpGet]
        [Route("join/{num}")]
        public IActionResult Join(int num){
            IsLoggedIn();
            
            int? uid = HttpContext.Session.GetInt32("userid");
            Rsvp newone = new Rsvp();
            newone.UserId = (int)uid;
            newone.EventId = num;
            _context.rsvps.Add(newone);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("leave/{num}")]
        public IActionResult Leave(int num){
            IsLoggedIn();
            int? uid = HttpContext.Session.GetInt32("userid");
            Rsvp removing = _context.rsvps.SingleOrDefault(rs => rs.EventId == num && rs.UserId == uid);
            _context.rsvps.Remove(removing);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("delete/{num}")]
        public IActionResult Delete(int num){
            IsLoggedIn();
            List<Event> list = _context.events.Include(x => x.Coming).ToList();
            Event todelete = new Event();
            foreach(Event item in list){
                if(item.EventId == num){todelete = item;}
            }
            foreach(Rsvp stuff in todelete.Coming){
                _context.rsvps.Remove(stuff);
            }
            _context.SaveChanges();
            _context.events.Remove(todelete);
            _context.SaveChanges();
            
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("event/{num}")]
        public IActionResult Event(int num){
            IsLoggedIn();
            if(unchecked( num != (int)num )){
                return RedirectToAction("Dashboard");
            }
            List<Event> list = _context.events.Include(x => x.Coming).ThenInclude(s => s.User).ToList();
            Event touse = new Event();
            foreach(Event item in list){
                if(item.EventId == num){touse = item;}
            }
            ViewBag.Wed = touse;
            ViewBag.uid = HttpContext.Session.GetInt32("userid");
            return View("One");
        }




    }
}
