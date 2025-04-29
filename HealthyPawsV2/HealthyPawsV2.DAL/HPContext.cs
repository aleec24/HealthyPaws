using Microsoft.EntityFrameworkCore;

namespace HealthyPawsV2.DAL
{
    public class HPContext : DbContext
    {
            public HPContext() { }

            public HPContext(DbContextOptions<HPContext> options) : base(options) { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);
                optionsBuilder.UseSqlServer();
            }

        public DbSet<PetBreed> PetBreeds { get; set; } //List of PetBreeds

        public DbSet<PetType> PetTypes { get; set; } //List of PetTypes

        public DbSet<PetFile> PetFiles { get; set; } //List of Petfiles

        public DbSet<LogReport> LogReports { get; set; }  //List of Logs

        public DbSet<Inventory> Inventories { get; set; }  //List of Inventories

        public DbSet<Document> Documents { get; set; }  //List of Documents

        public DbSet<AppointmentInventory> AppointmentInventories { get; set; }  //List of AppointmentInventories

        public DbSet<Appointment> Appointments { get; set; }  //List of Appointments

        public DbSet<Address> Addresses { get; set; }  //List of Addresses

        public DbSet<ApplicationUser> ApplicationUser { get; set; } //List of Application User

		public DbSet<Contact> Contacts { get; set; } //List of Contact

	}
}
