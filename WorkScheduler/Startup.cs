using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.SpaServices.StaticFiles;
using Microsoft.AspNetCore.WebSockets.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WorkScheduler;
using WorkScheduler.Models.Identity;
using WorkScheduler.Services;
using WorkScheduler.Services.Monitoring;

namespace WorkSheduler
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
            var logger = Logger.GetInstance();
            try
            {
                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

                ////Only for deploying
                //var context = new CustomAssemblyLoadContext();
                //context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));

                services.AddDbContext<WorkScheduler.Context>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

                services.AddIdentity<User, IdentityRole>(o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 4;
                })
                    .AddEntityFrameworkStores<WorkScheduler.Context>();

                services.ConfigureApplicationCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(180);
                });

                services.AddScoped<SchedulerService>();
                services.AddScoped<TicketService>();
                services.AddScoped<ReportService>();
                services.AddScoped<NotificationService>();
                services.AddScoped<ReportRenderService>();
                services.AddScoped<ChecklistService>();
                services.AddScoped<FileService>();
                services.AddScoped<WorkScheduler.Services.Monitoring.DictionaryService>();
                services.AddScoped<TalentedChildrenService>();

                var context = new CustomAssemblyLoadContext();
                context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));

                services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

                // In production, the Angular files will be served from this directory
                services.AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "ClientApp/dist";
                });
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var logger = Logger.GetInstance();
            try
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
                app.UseAuthentication();
                app.UseStaticFiles();
                app.UseSpaStaticFiles();

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
                        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    //spa.UseAngularCliServer(npmScript: "start");
                }
                });
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }
    }

    //Only for deploying
    internal class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }
        protected override IntPtr LoadUnmanagedDll(String unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }
    }
}
