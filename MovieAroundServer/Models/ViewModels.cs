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
}