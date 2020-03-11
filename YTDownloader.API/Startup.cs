using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using YoutubeExplode;
using YTDownloader.API.Domain.Entities;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Domain.Concrete;

namespace YTDownloader.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();
            services.AddAntiforgery();

            services.AddScoped<IYoutubeClient, YoutubeClient>();
            services.AddScoped<IYoutubeClientHelper>(s => new YoutubeClientHelper(new YoutubeClient(), env.WebRootPath + "\\ffmpeg.exe"));
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            //Clear wwwroot/DownloadedVideos dir
            string videosPath = env.WebRootPath + "\\DownloadedVideos";
            CleanDirectory.DeleteAllFiles(videosPath);
            CleanDirectory.DeleteAllSubDirs(videosPath);
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStaticFiles();
        }
    }
}
