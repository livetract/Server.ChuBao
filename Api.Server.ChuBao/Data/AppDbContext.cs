﻿using Microsoft.EntityFrameworkCore;

namespace Api.Server.ChuBao.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    }
}
