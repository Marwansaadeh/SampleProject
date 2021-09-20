using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BankAppData.Models
{
    public partial class BankAppDataContext : IdentityDbContext
    {
        public BankAppDataContext()
        {
        }

        public BankAppDataContext(DbContextOptions<BankAppDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Cards> Cards { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Dispositions> Dispositions { get; set; }
        public virtual DbSet<Loans> Loans { get; set; }
        public virtual DbSet<PermenentOrder> PermenentOrder { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<User> User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot Configuration = builder.Build();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
