using Data.Server.ChuBao.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Server.Chubao.Access
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }

    }
}
