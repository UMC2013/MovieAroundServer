﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieAroundServer.Models;

namespace MovieAroundServer.Controllers
{
    public class TheatersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Theaters/
        public ActionResult Index()
        {
            return View(db.Theaters.ToList());
        }

        // GET: /Theaters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theater theater = db.Theaters.Find(id);
            if (theater == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Movies = db.Movies.OrderBy(m => m.Title).ToList();

            return View(theater);
        }

        // GET: /Theaters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Theaters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TheaterId,Name,Latitude,Longitude,Address")] Theater theater)
        {
            if (ModelState.IsValid)
            {
                db.Theaters.Add(theater);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(theater);
        }

        // GET: /Theaters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theater theater = db.Theaters.Find(id);
            if (theater == null)
            {
                return HttpNotFound();
            }
            return View(theater);
        }

        // POST: /Theaters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TheaterId,Name,Latitude,Longitude,Address")] Theater theater)
        {
            if (ModelState.IsValid)
            {
                db.Entry(theater).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(theater);
        }

        // GET: /Theaters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theater theater = db.Theaters.Find(id);
            if (theater == null)
            {
                return HttpNotFound();
            }
            return View(theater);
        }

        // POST: /Theaters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Theater theater = db.Theaters.Find(id);
            db.Theaters.Remove(theater);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult AddShowTime(int theaterId, int movieId, string time)
        {
            TimeSpan timeShowTime = new TimeSpan();
            if (TimeSpan.TryParse(time, out timeShowTime))
            {
                ShowTime showTime = new ShowTime();

                Theater theater = db.Theaters.Find(theaterId);
                Movie movie = db.Movies.Find(movieId);

                showTime.Movie = movie;
                showTime.Theater = theater;
                showTime.Time = timeShowTime;

                db.ShowTimes.Add(showTime);
                db.SaveChanges();

                return Json(true);
            }
            else
                return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveShowTime(int showTimeId)
        {
            ShowTime showTime = db.ShowTimes.Find(showTimeId);
            db.ShowTimes.Remove(showTime);
            db.SaveChanges();

            return Json(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
