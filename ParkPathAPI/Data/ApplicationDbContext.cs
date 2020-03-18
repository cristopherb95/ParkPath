using System;
using Microsoft.EntityFrameworkCore;
using ParkPathAPI.Models;

namespace ParkPathAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<NationalPark> NationalParks { get; set; }
    }
}
