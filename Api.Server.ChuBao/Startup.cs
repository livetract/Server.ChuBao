using Api.Server.ChuBao.Data;
using Api.Server.ChuBao.IRepositories;
using Api.Server.ChuBao.ExtendeConfigs;
using Api.Server.ChuBao.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Api.Server.ChuBao.Services;
using System.Collections.Generic;

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

            services.AddDbContext<AppDbContext>(
                options =>
                options.UseSqlServer(Configuration.GetConnectionString("ChuBaoDB")));

            services.AddDbContext<IdDbContext>(
                options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityDB")));

            services.ConfigureIdentity();
            services.ConfigureJwt(Configuration);

            services.AddSwaggerGen(api =>
            {
                api.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "使用JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                api.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                            Scheme = "Oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                api.SwaggerDoc("v1", new OpenApiInfo { Title = "Service For ChuBao", Version = "v1" });
            });

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

            app.UseHttpsRedirection();
            app.UseCors("api");
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}