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
        public DbSet<Mark> Marks { get; set; }

        //public DbSet<ContactRecord> ContactRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 中间表操作
            //modelBuilder.Entity<ContactRecord>()
            //    .HasKey(cr => new {cr.ContactId, cr.RecordId});

            //modelBuilder.Entity<ContactRecord>()
            //    .HasOne(cr => cr.Contact)
            //    .WithMany(c => c.ContactRecords)
            //    .HasForeignKey(cr => cr.ContactId);

            //modelBuilder.Entity<ContactRecord>()
            //    .HasOne(cr => cr.Record)
            //    .WithMany(r => r.ContactRecords)
            //    .HasForeignKey(cr => cr.RecordId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
