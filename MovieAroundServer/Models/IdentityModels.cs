using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace MovieAroundServer.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<ShowTime> ShowTimes { get; set; }
    }

    public partial class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }

    public partial class Movie
    {
        public Movie()
        {
            this.Genres = new List<Genre>();
        }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<ShowTime> ShowTimes { get; set; }
    }

    public partial class Theater
    {
        public int TheaterId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public virtual ICollection<ShowTime> ShowTimes { get; set; }
    }

    public partial class ShowTime
    {
        public int ShowTimeId { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Theater Theater { get; set; }
        public System.TimeSpan Time { get; set; }
    }
}