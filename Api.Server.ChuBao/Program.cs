using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;

namespace Api.Server.ChuBao
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(path: "Logs/applog.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            try
            {
                Log.Information("Starting web application...");

                var builder = WebHost.CreateDefaultBuilder(args);
                
                _ = builder.UseSerilog();

                builder.UseStartup<Startup>();
                
                var app = builder.Build();

                // 👇不要改成异步；
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly...");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}