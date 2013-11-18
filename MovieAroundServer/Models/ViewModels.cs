using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieAroundServer.Models
{
    public class EditMovieViewModel
    {
        public Movie Movie { get; set; }
        public IEnumerable<SelectListItem> Genres { get; set; }
        public List<int> SelectedGenres { get; set; }
    }

    public class MovieTheaterViewModel
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string MovieGenre { get; set; }
        public List<TheaterViewModel> Theaters { get; set; }
    }

    public class TheaterViewModel
    {
        public int TheaterId { get; set; }
        public string Name { get; set; }
    }

    public class GenreViewModel
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
    }
}