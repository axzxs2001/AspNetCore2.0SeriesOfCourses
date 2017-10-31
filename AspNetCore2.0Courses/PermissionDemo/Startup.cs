using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace PermissionDemo
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

            services.AddAuthentication(opt =>
            {
               // opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme= CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme= CookieAuthenticationDefaults.AuthenticationScheme; ;
                opt.DefaultSignInScheme= CookieAuthenticationDefaults.AuthenticationScheme; 

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,opt=> {
                opt.LoginPath = new PathString("/login");                
                opt.AccessDeniedPath = new PathString("/idenied");
                opt.LogoutPath = new PathString("/logout");
                opt.Cookie.Path = "/";
            });

            services.AddMvc();


        }

  
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
