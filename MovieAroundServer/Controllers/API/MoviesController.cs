using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MovieAroundServer.Models;

namespace MovieAroundServer.Controllers.API
{
    public class MoviesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET api/Movies
        public IQueryable<Movie> GetMovies()
        {
            return db.Movies;
        }

        public IEnumerable<MovieTheaterViewModel> GetMoviesByLocation(double latitude, double longitude, string genres)
        {
            //todo: receber tolerancia como parametro, mas default 1

            //get theaters
            var theaters = db.Theaters.Where(t => 
                (t.Latitude >= latitude - 1 && t.Latitude <= latitude + 1) &&
                (t.Longitude >= longitude -1 && t.Longitude <= longitude + 1)
                );

            //haversine

            //get genres
            List<Genre> selectedGenres = new List<Genre>();
            var listGenres = genres.Split(new char[] { ',' });
            foreach (var s in listGenres)
            {
                Genre genre = new Genre();
                int genreId = 0;
                if (Int32.TryParse(s, out genreId))
                    genre = db.Genres.Find(genreId);
                else
                    genre = db.Genres.FirstOrDefault(g => g.Name == s);

                if (genre != null)
                    selectedGenres.Add(genre);
            }

            //match
            List<MovieTheaterViewModel> movies = new List<MovieTheaterViewModel>();
            foreach (var theater in theaters)
            {
                foreach (var s in theater.ShowTimes)
                {
                    if (s.Movie.Genres.Count(g => selectedGenres.Count(gg => gg.GenreId == g.GenreId) > 0) > 0)
                    {
                        MovieTheaterViewModel movie = new MovieTheaterViewModel();

                        TheaterViewModel theaterModel = new TheaterViewModel();
                        theaterModel.TheaterId = theater.TheaterId;
                        theaterModel.Name = theater.Name;

                        if (movies.Count(m => m.MovieId == s.Movie.MovieId) == 0)
                        {
                            movie.MovieId = s.Movie.MovieId;
                            movie.MovieTitle = s.Movie.Title;
                            movie.MovieGenre = s.Movie.Genres.First().Name;
                            movie.Theaters = new List<TheaterViewModel>();
                            movie.Theaters.Add(theaterModel);
                            movies.Add(movie);
                        }
                        else
                        {
                            movie = movies.FirstOrDefault(m => m.MovieId == s.Movie.MovieId);
                            movie.Theaters.Add(theaterModel);
                        }
                        
                    }
                }
            }

            return movies.AsEnumerable();
            
        }

        // GET api/Movies/5
        [ResponseType(typeof(Movie))]
        public IHttpActionResult GetMovie(int id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // PUT api/Movies/5
        public IHttpActionResult PutMovie(int id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.MovieId)
            {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Movies
        [ResponseType(typeof(Movie))]
        public IHttpActionResult PostMovie(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = movie.MovieId }, movie);
        }

        // DELETE api/Movies/5
        [ResponseType(typeof(Movie))]
        public IHttpActionResult DeleteMovie(int id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
            db.SaveChanges();

            return Ok(movie);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(int id)
        {
            return db.Movies.Count(e => e.MovieId == id) > 0;
        }
    }
}