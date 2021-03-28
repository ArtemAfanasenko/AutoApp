using AutoApp.Models;
using System.Data.Entity;

namespace AutoApp.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("MyConnection")
        {
        }
        public DbSet<FuelModel> Fuel { get; set; }
        public DbSet<TypeCarModel> TypeCar { get; set; }
        public DbSet<BrandModel> Brand { get; set; }
        public DbSet<CarModel> Car { get; set; }
    }
}
