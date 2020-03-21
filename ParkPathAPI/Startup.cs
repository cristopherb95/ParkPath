using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ParkPathAPI.Data;
using ParkPathAPI.Mapper;
using ParkPathAPI.Repository;
using ParkPathAPI.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkPathAPI
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
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddScoped<INationalParkRepository,NationalParkRepository>();
            services.AddScoped<ITrailRepository,TrailRepository>();
            services.AddAutoMapper(typeof(ParkPathMappings));
            
            // VERSIONING
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            // services.AddSwaggerGen(options =>
            // {
            //     options.SwaggerDoc("ParkOpenAPISpec", new OpenApiInfo()
            //     {
            //         Title = "ParkPath API",
            //         Version = "1"
            //     });
            //     // options.SwaggerDoc("ParkOpenAPISpecTrails", new OpenApiInfo()
            //     // {
            //     //     Title = "ParkPath API Trails",
            //     //     Version = "1"
            //     // });
            //     
            //     options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ParkPathAPI.xml"));
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            // app.UseSwaggerUI(options =>
            // {
            //     options.SwaggerEndpoint("/swagger/ParkOpenAPISpec/swagger.json", "Park Path API");
            //    // options.SwaggerEndpoint("/swagger/ParkOpenAPISpecTrails/swagger.json", "Park Path API Trails");
            // });
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                        desc.GroupName.ToUpperInvariant());
                }
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
