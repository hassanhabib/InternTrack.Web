// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using InternTrack.Portal.Web.Models.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RESTFulSense.Clients;

namespace InternTrack.Portal.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            AddHttpClient(services);
            AddRootDirectory(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        private void AddHttpClient(IServiceCollection services)
        {
            services.AddHttpClient<IRESTFulApiFactoryClient, RESTFulApiFactoryClient>(client =>
            {
                LocalConfigurations localConfigurations = Configuration.Get<LocalConfigurations>();
                string apiUrl = localConfigurations.ApiConfigurations.Url;
                client.BaseAddress = new Uri(apiUrl);
            });
        }

        private static void AddRootDirectory(IServiceCollection services)
        {
            services.AddRazorPages(options =>
            {
                options.RootDirectory = "/Views/Pages";
            });
        }
    }
}
