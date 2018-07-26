using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shiro.Models;
using Shiro.Services;

namespace Shiro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async ctx =>
                        {
                            var _context = ctx.HttpContext.RequestServices.GetRequiredService<SystemContext>();

                            ClaimsIdentity identity = ctx.Principal.Identity as ClaimsIdentity;
                            Claim claim = identity.Claims.FirstOrDefault(x => x.Type == "userId");
                            Guid userId = new Guid(claim.Value);

                            if (userId != null)
                            {
                                var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId && u.IsAdministrator);
                                if (user != null)
                                {
                                    identity.AddClaim(new Claim(ClaimTypes.Role, "administrator"));
                                }
                            }
                        }
                    };
                });

            services.AddDbContext<SystemContext>(options => options.UseInMemoryDatabase("System"));
            services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("Application"));

            // services.AddDbContext<SystemContext>(options =>
            //     options
            //         .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            //         .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            // services.AddDbContext<ApplicationContext>(options =>
            //     options
            //         .UseSqlServer(Configuration.GetConnectionString("ApplicationConnection"))
            //         .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "WEB API", Version = "v1" });
            });

            services.AddAutoMapper();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/packages/shiro-app/build";
            });
        }

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WEB API V1");
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}
