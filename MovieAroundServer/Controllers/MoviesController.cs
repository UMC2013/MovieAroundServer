using System;
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
    public class MoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Movies/
        public ActionResult Index()
        {
            return View(db.Movies.ToList());
        }

        // GET: /Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: /Movies/Create
        public ActionResult Create()
        {
            var genres = db.Genres.ToList();

            ViewBag.Genres = new MultiSelectList(genres, "GenreId", "Name");

            return View();
        }

        // POST: /Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MovieId,Title,Synopsis")] Movie movie, int[] genres)
        {
            if (ModelState.IsValid)
            {
                foreach (var genreId in genres)
                {
                    var genre = db.Genres.Find(genreId);
                    movie.Genres.Add(genre);
                }
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: /Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            EditMovieViewModel model = new EditMovieViewModel();
            model.Movie = movie;
            model.Genres = new SelectList(db.Genres.ToList(), "GenreId", "Name");
            model.SelectedGenres = movie.Genres.Select(g => g.GenreId).ToList();

            return View(model);
        }

        // POST: /Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieId,Title,Synopsis")] Movie movie, int[] SelectedGenres)
        {
            if (ModelState.IsValid)
            {
                Movie updatedMovie = db.Movies.Find(movie.MovieId);
                updatedMovie.Genres.Clear();
                updatedMovie.Title = movie.Title;
                updatedMovie.Synopsis = movie.Synopsis;

                List<Genre> genres = new List<Genre>();
                foreach (var genreId in SelectedGenres)
                {
                    genres.Add(db.Genres.Find(genreId));
                }

                updatedMovie.Genres = genres;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: /Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: /Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
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
