using Microsoft.EntityFrameworkCore;
using VladimirVAZ3.Scripts;

namespace VladimirVAZ3.Data
{
    class ConnectTablesClient : DbContext
    {
        public DbSet<Client>? Client {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(StaticValues.Instance.Path);
    }

    class ConnectTablesUsers : DbContext
    {
        public DbSet<Users>? Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(StaticValues.Instance.Path);
    }

    class ConnectTablesTypesVehicles : DbContext
    {
        public DbSet<TypesVehicles>? TypesVehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(StaticValues.Instance.Path);
    }

    class ConnectTablesVehicles : DbContext
    {
        public DbSet<Vehicles>? Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(StaticValues.Instance.Path);
    }

    class ConnectTablesImagesVehicles : DbContext
    {
        public DbSet<ImagesVehicles>? ImagesVehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(StaticValues.Instance.Path);
    }

    class ConnectTablesPrice : DbContext
    {
        public DbSet<Prices>? Prices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(StaticValues.Instance.Path);
    }

    class ConntectTablesRents : DbContext
    {
        public DbSet<Rents>? Rents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(StaticValues.Instance.Path);
    }
}
