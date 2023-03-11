using Api.Server.ChuBao.Commons;
using Core.Server.ChuBao.Models;
using Data.Server.Chubao.Access;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;

namespace Api.Server.ChuBao
{
    public static class ServicesExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services) 
        {
            services.AddIdentity<IdentityUser, IdentityRole>(
                options => options
                .SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<IdDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Name));
            var jwtOptions = configuration.GetSection(JwtOptions.Name).Get<JwtOptions>();

            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(
                options =>
                {
                    options.SaveToken = true;
                    options.SecurityTokenValidators.Clear();

                    options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        //ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256},
                        //ValidTypes = new[] {JwtConstants.HeaderType},
                        ValidateActor = true,
                        ValidateTokenReplay = true,

                        ValidateIssuer = true,
                        //ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = false, //dev
                        //ValidAudience = jwtOptions.Audience,

                        ValidateLifetime = true,

                        RequireSignedTokens = true,
                        RequireExpirationTime = false,  // dev

                        ValidateIssuerSigningKey = true,
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                        //NameClaimType = JwtClaimTypes.Name,
                        //RoleClaimType = JwtClaimTypes.Role,
                        // 不要在这里面赋值，主要是指定要验证哪些

                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        public static void ConfigureSwaggerCust(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "使用JWT，格式 Bearer[space]Token like 'Bearer 123456'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement()
                    {
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

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Service For ChuBao", Version = "v1" });
            });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(
                error =>
                {
                    error.Run(async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.ContentType = "application/json";
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null)
                        {
                            Log.Error($"Error: {contextFeature.Error}");
                            await context.Response.WriteAsync(
                                new Error
                                {
                                    StatusCode = context.Response.StatusCode,
                                    Message = "服务器出现了错误，请重新尝试！"
                                }.ToString()); ;
                        }
                    } );
                } );
        }

    }
}
