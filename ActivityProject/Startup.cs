using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityProject.Data;
using ActivityProject.Middleware;
using ActivityProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rotativa.AspNetCore;

namespace ActivityProject
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ActivityProjectDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("projekat1DB")));

            services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.FromHours(24));
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
     .AddCookie(options => {
         options.LoginPath = "/Account/Index/";
         options.Cookie.Expiration = TimeSpan.FromHours(24);
     });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IActivityData, SqlActivityData>();
            services.AddScoped<ICourseData, SqlCourseData>();
            services.AddScoped<IActivityTypeData, SqlActivityTypeData>();
            services.AddScoped<IStudentData, SqlStudentData>();
            services.AddScoped<IProfessorData, SqlProfessorData>();
            services.AddScoped<ICourseStudentData, SqlCourseStudentData>();
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver
                    = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling =
Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            });
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseSession();
            app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());
            app.UseStaticFiles();
            app.UseNodeModules(env.ContentRootPath);
            app.UseMvc(ConfigureRoutes);

            //RotativaConfiguration.Setup(env);

        }

        private void ConfigureRoutes(IRouteBuilder obj)
        {
            obj.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}/{id1?}");
        }
    }
}
