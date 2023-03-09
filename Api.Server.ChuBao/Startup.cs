using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Api.Server.ChuBao.Utilities;
using Data.Server.Chubao.Access;
using Data.Server.Chubao.Repositories;
using System;

namespace Api.Server.ChuBao
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("api", builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });


            // 分开后，1. 要使用指定程序集，startup是目标， 2. 使用pmc来做迁移，不要忘记-Context AppDbContext之类的
            services.AddDbContext<AppDbContext>(
                options =>
                options.UseSqlServer(Configuration.GetConnectionString("ChuBaoDB"), 
                b => 
                { 
                    b.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: new int[] {2});
                    b.MigrationsAssembly(this.GetType().Assembly.FullName);
                }));

            services.AddDbContext<IdDbContext>(
                options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityDB"),
                 b =>
                 {
                     b.EnableRetryOnFailure(
                     maxRetryCount: 5,
                     maxRetryDelay: TimeSpan.FromSeconds(30),
                     errorNumbersToAdd: new int[] { 2 });
                     b.MigrationsAssembly(this.GetType().Assembly.FullName);
                 }));

            services.ConfigureIdentity();
            services.ConfigureAuthentication(Configuration);
            services.ConfigureSwaggerCust();


            services.AddAutoMapper(typeof(MapperProfile));


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthManager,  AuthManager>();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment evn)
        {
            if (!evn.IsDevelopment())
            {
                app.UseExceptionHandler();

                // 👇严格安全传输;
                app.UseHsts();
            }
            if (evn.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSerilogRequestLogging();
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();
            app.UseCors("api");
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}