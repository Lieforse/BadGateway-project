using BadGateway.DataAccess;
using BadGateway.HostedServices;
using BadGateway.Services;
using BadGateway.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BadGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions()
                    .Configure<EmailNotificationOptions>(Configuration.GetSection("EmailNotification"))
                    .AddRazorPages();

            if (Environment.IsProduction())
            {
                services.AddDbContext<AppDbContext>(x => x.UseMySql(Configuration.GetConnectionString("BadGatewayDb")));
                services.AddHostedService<MigratorHostedService>();
            }
            else
            {
                services.AddDbContext<AppDbContext>(x => x.UseInMemoryDatabase("badgatewaydatabase"));
            }

            services.AddTransient<IEmailService, EmailService>()
                    .AddHostedService<PostGrabberService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
