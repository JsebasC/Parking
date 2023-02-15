
using Microsoft.EntityFrameworkCore;
using Parking.API.Domain.Entities;

namespace Parking.API.Infrastructure.Data
{
    public class ParkingContext : DbContext
    {        

        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options){     }

        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<ParkingSpaces> ParkingSpaces { get; set; }
        public DbSet<Domain.Entities.Parking>  Parking { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }

            modelBuilder.HasDefaultSchema("dbo");            
            modelBuilder.Entity<Parking.API.Domain.Entities.Parking>()
               .Property(p => p.TotalValue)
               .HasPrecision(18, 2);

            modelBuilder.Entity<Parking.API.Domain.Entities.ParkingRate>()
             .Property(p => p.Value)
             .HasPrecision(18, 2);
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
