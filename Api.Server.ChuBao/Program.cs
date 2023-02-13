using Serilog;

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

                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseSerilog();

                var startup = new Startup(builder.Configuration);
                startup.ConfigureServices(builder.Services);

                var app = builder.Build();
                startup.Configure(app, app.Environment);

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