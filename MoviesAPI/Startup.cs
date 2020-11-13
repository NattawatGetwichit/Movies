using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoviesAPI.Areas.ApiV1.Services.FileStorageServices;
using MoviesAPI.Areas.ApiV1.Services.GenreServices;
using MoviesAPI.Areas.ApiV1.Services.HostedServices;
using MoviesAPI.Areas.ApiV1.Services.MovieServices;
using MoviesAPI.Areas.ApiV1.Services.PersonServices;
using MoviesAPI.Data;
using MoviesAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

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
            services.AddHealthChecks().AddDbContextCheck<BulkDBContext>(tags: new[] { "ready" });
            //------End: HealthChecks------

            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<BulkDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BulkConnection")));

            //------Identity User------
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();
            //------End: Identity User------

            //------Authentication------
            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                        ClockSkew = TimeSpan.Zero
                    }
                );
            //------End: Authentication------

            services.AddHttpContextAccessor();
            services.AddResponseCaching();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "CoreAPI" });

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                    });
            });

            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IMovieService, MovieService>();

            services.AddTransient<IFileStorageService, InAppStorageService>();

            services.AddTransient<IHostedService, WriteToFileHostedService>();
            services.AddTransient<IHostedService, MovieInTheatersService>();
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

            //------Authentication------
            app.UseAuthentication();
            //------End: Authentication------

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