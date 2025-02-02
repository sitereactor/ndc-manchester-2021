using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ingredients.Protos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.Protos;

namespace Frontend
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
            services.AddGrpcClient<IngredientsService.IngredientsServiceClient>((provider, options) =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var uri = config.GetServiceUri("Ingredients", "https");
                options.Address = uri;
            });
            services.AddGrpcClient<OrdersService.OrdersServiceClient>(((provider, options) =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var uri = config.GetServiceUri("Orders", "https")
                          ?? new Uri("https://localhost:5005");
                options.Address = uri;
            }));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
