using Data.Server.Chubao.Entities;
using Data.Server.ChuBao.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Server.Chubao.Access
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Record> Records { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
