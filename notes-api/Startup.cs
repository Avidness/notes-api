using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using notes_api.DAL.EFCore;
using notes_api.DAL.Repositories;
using notes_api.Models.Domain;

namespace notes_api
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
            services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy",
                builder =>
                {
                    builder.WithOrigins(Configuration["AllowedCORSOrigin"]);
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                    builder.AllowAnyHeader();
                });
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            
            services.AddScoped<ItemRepository, ItemRepository>();
            services.AddScoped<CategoryRepository, CategoryRepository>();
            services.AddDbContext<MainContext>(opt => opt.UseInMemoryDatabase("notes_db"));
            //services.AddDbContext<MainContext>(options => 
             //   options.UseSqlServer(Configuration["DefaultConnection"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, MainContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // TODO: add logging

            AddTestData(context);

            app.UseCors("CORSPolicy");
            app.UseMvc();
        }

        private static void AddTestData(MainContext context)
        {
          var cat1 = new Category
          {
              Id = 1,
              Label = "test category 1"
          };
      
          context.Categories.Add(cat1);
      
          var item1 = new Item
          {
              Id = 1,
              Category = cat1,
              Label = "test item 1",
              Description = "Description 1"
          };
      
          context.Items.Add(item1);
      
          context.SaveChanges();
      }
    }
}
