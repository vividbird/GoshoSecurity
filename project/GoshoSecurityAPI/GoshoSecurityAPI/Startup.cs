namespace GoshoSecurityAPI
{
    using AutoMapper;
    using GoshoSecurity.Data;
    using GoshoSecurity.Data.Interfaces;
    using GoshoSecurity.Data.Repository;
    using GoshoSecurity.Infrastructure;
    using GoshoSecurity.Infrastructure.CloudinaryConfig;
    using GoshoSecurity.Infrastructure.CognitiveServicesFaceConfig;
    using GoshoSecurity.Infrastructure.Mapping;
    using GoshoSecurity.Models;
    using GoshoSecurity.Services;
    using GoshoSecurity.Services.Interfaces;
    using GoshoSecurityAPI.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Text;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GoshoSecurityDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<GoshoSecurityUser, IdentityRole>()
             .AddEntityFrameworkStores<GoshoSecurityDbContext>().AddDefaultTokenProviders()
             .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            services.Configure<CognitiveServicesFaceConfig>(Configuration.GetSection("CognitiveServicesFace"));
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.Configure<Jwt>(Configuration.GetSection("Jwt"));

            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IAccountService, AccountService>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
            });

            services.AddAuthentication().
                AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseAuthentication();

            DataInitializer.SeedRoles(services).Wait();

        }
    }
}
