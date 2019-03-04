using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using twe_todo_app.Data;
using twe_todo_app.Models.Keys;

namespace twe_todo_app {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseSqlServer(
                //    Configuration.GetConnectionString("DefaultConnection")));
                options.UseMySql(Configuration.GetConnectionString("MySQLConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
            //services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication().AddTwitter(twitterOptions => {
                twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                twitterOptions.Events.OnCreatingTicket = context => {
                    var identity = (ClaimsIdentity)context.Principal.Identity;
                    identity.AddClaim(new Claim(nameof(context.AccessToken), context.AccessToken));
                    identity.AddClaim(new Claim(nameof(context.AccessTokenSecret), context.AccessTokenSecret));

                    return Task.CompletedTask;
                };
            });

            //AddHttpContextAccessorの追加、これでどこからでもアクセスできるようになるっぽい
            //https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/http-context?view=aspnetcore-2.2#use-httpcontext-from-custom-components
            services.AddHttpContextAccessor();

            services.Configure<IdentityOptions>(options => {
                //
            });

            services.Configure<ConsumerKeys>(options => {
                options.Key = Configuration["Authentication:Twitter:ConsumerKey"];
                options.Secret = Configuration["Authentication:Twitter:ConsumerSecret"];
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            } else {
                app.UseExceptionHandler("/Todo/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Todo}/{action=Index}/{id?}");
            });
        }
    }
}
