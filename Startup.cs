using Amazon.DynamoDBv2;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OrderApi.API.Extensions;
using OrderAPI.Infrastructure.Core.SnsClasses;
using OrderApi.Domain.Commands;
using OrderAPI.Domain.Events;
using OrderApi.Domain.Queries;
using OrderAPI.Infrastructure.Core;
using OrderApi.Infrastructure.EventStore;
using OrderApi.Infrastructure.Persistence;
using OrderApi.Infrastructure.Repositores;
using OrderApi.OrderAPI.Domain.Handlers;

namespace OrderApi
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

            services.AddControllers(o=> o.InputFormatters.Insert(o.InputFormatters.Count, new TextPlainInputFormatter()))
                .AddNewtonsoftJson(x =>
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddInfrastructure(Configuration);
            // JSON Serializer
            services.AddSingleton<IEventSerializer,JsonEventSerializer>();
            
            // Swagger config
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderApi", Version = "v1" });
            });
            // Versioning setup
            services.AddApiVersioning(conf =>
            {
                conf.DefaultApiVersion = new ApiVersion(1, 0);
                conf.AssumeDefaultVersionWhenUnspecified = true;
                conf.ReportApiVersions = true;
            });
            // CORS
            services.AddCors(ops =>
            {
                ops.AddPolicy("CorsPolicy", policy => 
                {
                    policy.AllowAnyMethod().AllowAnyHeader()
                        .WithOrigins("http://localhost:3000", "http://localhost:5000");
                });
            });
          
            services.AddMediatR(typeof(Startup).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
