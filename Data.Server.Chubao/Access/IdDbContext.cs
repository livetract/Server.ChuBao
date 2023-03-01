using Data.Server.Chubao.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Server.Chubao.Access
{
    public class IdDbContext : IdentityDbContext
    {
        public IdDbContext(DbContextOptions<IdDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());


            base.OnModelCreating(builder);
        }
    }
}
