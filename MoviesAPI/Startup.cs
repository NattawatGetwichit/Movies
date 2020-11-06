using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoviesAPI.Areas.ApiV1.Services.FileStorageServices;
using MoviesAPI.Areas.ApiV1.Services.GenreServices;
using MoviesAPI.Areas.ApiV1.Services.HostedServices;
using MoviesAPI.Areas.ApiV1.Services.MovieServices;
using MoviesAPI.Areas.ApiV1.Services.PersonServices;
using MoviesAPI.Data;
using MoviesAPI.Helpers;

namespace MoviesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //------Allow Origins------
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
                builder.WithExposedHeaders("totalAmountRecords");
                builder.WithExposedHeaders("totalAmountPages");
                builder.WithExposedHeaders("currentPage");
                builder.WithExposedHeaders("recordsPerPage");
            }));
            //------End: Allow Origins------

            //------HealthChecks------
            services.AddHealthChecks().AddDbContextCheck<AppDBContext>(tags: new[] { "ready" });
            //------End: HealthChecks------

            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHttpContextAccessor();
            services.AddResponseCaching();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddSwaggerGen();

            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IMovieService, MovieService>();

            services.AddTransient<IFileStorageService, InAppStorageService>();

            services.AddTransient<IHostedService, WriteToFileHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "MoviesAPI v1"));

            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseResponseCaching();

            //app.UseAuthentication();

            app.UseAuthorization();

            //------Allow Origins------
            app.UseCors("MyPolicy");
            //------End: Allow Origins------

            //------HealthChecks------
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckResponseWriter.WriteResponseReadiness,
                    Predicate = (check) => check.Tags.Contains("ready")
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckResponseWriter.WriteResponseLiveness,
                    Predicate = (check) => !check.Tags.Contains("ready")
                });
            });
            //------End: HealthChecks------

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}