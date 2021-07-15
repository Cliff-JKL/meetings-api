using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using meetingsAPI.EF;
using meetingsAPI.Services.MeetingService;
using meetingsAPI.Services.AuthorizationService;
using meetingsAPI.Extensions;
using meetingsAPI.Middleware;

namespace meetingsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Connection = Configuration.GetConnectionString("MeetingsSQLite");
        }

        public static string Connection { get; set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IMeetingContext, MeetingContext>(options => options.UseSqlite(Connection));
            services.AddControllers();
            services.AddTransient<IMeetingService, MeetingService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddHttpContextAccessor();
            services.AddJwtAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSimplExceptionHandler();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
