namespace MovieAroundServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShowTime : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TheaterMovies", "Theater_TheaterId", "dbo.Theaters");
            DropForeignKey("dbo.TheaterMovies", "Movie_MovieId", "dbo.Movies");
            DropIndex("dbo.TheaterMovies", new[] { "Theater_TheaterId" });
            DropIndex("dbo.TheaterMovies", new[] { "Movie_MovieId" });
            CreateTable(
                "dbo.ShowTimes",
                c => new
                    {
                        ShowTimeId = c.Int(nullable: false, identity: true),
                        Time = c.Time(nullable: false, precision: 7),
                        Movie_MovieId = c.Int(),
                        Theater_TheaterId = c.Int(),
                    })
                .PrimaryKey(t => t.ShowTimeId)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieId)
                .ForeignKey("dbo.Theaters", t => t.Theater_TheaterId)
                .Index(t => t.Movie_MovieId)
                .Index(t => t.Theater_TheaterId);
            
            DropTable("dbo.TheaterMovies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TheaterMovies",
                c => new
                    {
                        Theater_TheaterId = c.Int(nullable: false),
                        Movie_MovieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Theater_TheaterId, t.Movie_MovieId });
            
            DropForeignKey("dbo.ShowTimes", "Theater_TheaterId", "dbo.Theaters");
            DropForeignKey("dbo.ShowTimes", "Movie_MovieId", "dbo.Movies");
            DropIndex("dbo.ShowTimes", new[] { "Theater_TheaterId" });
            DropIndex("dbo.ShowTimes", new[] { "Movie_MovieId" });
            DropTable("dbo.ShowTimes");
            CreateIndex("dbo.TheaterMovies", "Movie_MovieId");
            CreateIndex("dbo.TheaterMovies", "Theater_TheaterId");
            AddForeignKey("dbo.TheaterMovies", "Movie_MovieId", "dbo.Movies", "MovieId", cascadeDelete: true);
            AddForeignKey("dbo.TheaterMovies", "Theater_TheaterId", "dbo.Theaters", "TheaterId", cascadeDelete: true);
        }
    }
}
