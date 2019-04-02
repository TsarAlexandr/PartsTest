using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.XPath;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace PartsTest
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
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            var assembly = Assembly.Load(new AssemblyName("Module"));
            var resourceNames = assembly.GetManifestResourceNames();
            Configuration["MainLayout"] = GetLayoutName(resourceNames) ?? "_Layout";
            services.AddMvc()
                .AddApplicationPart(assembly).AddRazorOptions(
                    o =>
                    {
                        o.FileProviders.Add(new EmbeddedFileProvider(assembly, assembly.GetName().Name));
                    }
                );
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
            {
                HotModuleReplacement = true
            });

            app.UseStatusCodePages();
            app.UseStaticFiles();

            //app.UseMvc();
            app.UseMvcWithDefaultRoute();


        }

        private string GetLayoutName(string[] resources)
        {
            var shared = Configuration["SharedFolderName"];
            
            var layout = resources.FirstOrDefault(x => x.ToLower().Contains("layout") && x.Contains(shared));

            if (layout == null)
                return null;

            var first = layout.IndexOf(shared) + shared.Length;
            var second = layout.LastIndexOf('.');
            
            return layout.Substring(first,second - first );

        }
    }
}
