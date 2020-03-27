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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IO;
using System.Net;
using YTDownloader.API.Models;

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
        public object EncodingConfiguration { get; private set; }
        public object DownloadrConfiguration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();
            services.AddAntiforgery();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options => {
                            options.TokenValidationParameters = new TokenValidationParameters() 
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:TokenKey").Value)),
                                ValidateIssuer = false,
                                ValidateAudience = false,
                            };
                        });

            services.AddScoped<IYoutubeClient, YoutubeClient>();
            services.AddScoped<IYoutubeClientHelper>(s => new YoutubeClientHelper(new YoutubeClient(), env.WebRootPath + "\\ffmpeg.exe"));
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAccountPermissionChecker, AccountPermissionChecker>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            //Configuration
            string videosPath = env.WebRootPath + "\\DownloadedVideos"; // Place where your videos will be saved on the server
            string ffmpegLocation = env.WebRootPath + "\\ffmpeg.exe";  //Location to your ffmpeg.exe
            string ffmpegDownloadLink = "https://drive.google.com/u/0/uc?id=1kuiOY4_uAZxgp04YoB6DGL-tWiv5T-VD&export=download";  //Location where ffmpeg.exe can be downloaded in case if it will be missing at server startup
            DownloaderConfiguration.SetSettings(ffmpegLocation, videosPath, ffmpegDownloadLink);
            //Clear wwwroot/DownloadedVideos dir or create on if not exist
            if (File.Exists(DownloaderConfiguration.videoDownloadPath))
            {
                CleanDirectory.DeleteAllFiles(videosPath);
                CleanDirectory.DeleteAllSubDirs(videosPath);
            }
            else
            {
                Directory.CreateDirectory(videosPath);
            }
            //Check if ffmpeg exists if not download it
            if (!File.Exists(DownloaderConfiguration.ffmpegLocation))
            {
                WebClient web = new WebClient();
                web.DownloadFile(DownloaderConfiguration.ffmpegDownloadLink, DownloaderConfiguration.ffmpegLocation);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();

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
