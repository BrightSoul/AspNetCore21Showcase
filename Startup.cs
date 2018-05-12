using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCore21Showcase.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using AspNetCore21Showcase.Hubs;
using System.Net.Http;
using AspNetCore21Showcase.MessageHandlers;
using Polly;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace AspNetCore21Showcase
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHsts(options => {
                options.IncludeSubDomains = true;
                options.Preload = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            services.AddHttpsRedirection(options => {
                //options.HttpsPort = 5001;
                options.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            //Configurazione della pipeline di HttpClient
            services.AddHttpClient("ClientForThirdPartyWebApi", client => {
                //Qui eventuale inizializzazione del client
                //client.BaseAddress = new Uri("https://localhost:5001/");
                //client.DefaultRequestHeaders.Add("MyHeader", "Value");
            })
            //Message handler personalizzato
            .AddHttpMessageHandler<LogMessageHandler>()
            //Message handler di Polly
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new TimeSpan[]
            {
                TimeSpan.FromMilliseconds(50),
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromMilliseconds(1000),
            }));
            
            //services.AddTransient<RetryMessageHandler>();
            services.AddTransient<LogMessageHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<DrawHub>("/draw");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
