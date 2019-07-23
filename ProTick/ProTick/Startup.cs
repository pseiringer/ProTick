using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProTickDatabase;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore;
using ProTick.Singletons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.AspNetCore.Http;

namespace ProTick
{
    public class Startup
    {
        public Startup(IConfiguration conf, IHostingEnvironment env)
        {
            Configuration = conf;
            this.env = env;
        }

        private IHostingEnvironment env;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtAuth = Configuration.GetSection("JwtAuthentication");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<ProTickDatabaseContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDatabaseQueryManager, DatabaseQueryManager>();
            services.AddScoped<IResourceDTOConverter, ResourceDTOConverter>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var keyByteArray = System.Text.Encoding.UTF8.GetBytes(jwtAuth["SecurityKey"]);
                var signinKey = new SymmetricSecurityKey(keyByteArray);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = signinKey,
                    ValidAudience = jwtAuth["ValidAudience"],
                    ValidIssuer = jwtAuth["ValidIssuer"],
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
            });


            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();


            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.Options.StartupTimeout = new TimeSpan(0, 0, 80);
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            //(app.ApplicationServices.GetRequiredService<ProTickDatabaseContext>()).Database.EnsureCreated();


            app.UseCors("EnableCORS");
        }
    }
}
