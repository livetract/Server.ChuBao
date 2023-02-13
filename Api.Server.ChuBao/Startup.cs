using Serilog;

namespace Api.Server.ChuBao
{
    public class Startup
    {
        public ConfigurationManager Configuration { get; }

        public Startup(ConfigurationManager configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

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
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}