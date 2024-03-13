using CinemaSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Core
{
    public class CinemaSystemContext : DbContext
    {
        public CinemaSystemContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Seance> Seances { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<SeatType> SeatTypes { get; set; }
        public DbSet<Promo> Promos { get; set; }
        public DbSet<User> Userss { get; set; }
        public DbSet<Role> Roles { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CinemaSystem;Trusted_Connection=True;");
        //}

    }
}
