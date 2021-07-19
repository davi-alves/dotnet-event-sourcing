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
using OrderApi.API.Extensions;
using OrderApi.Domain.Commands;
using OrderAPI.Domain.Events;
using OrderApi.Domain.Queries;
using OrderAPI.Infrastructure.Core;
using OrderApi.Infrastructure.EventStore;
using OrderApi.Infrastructure.Persistence;
using OrderApi.Infrastructure.Repositores;

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

            services.AddControllers();
            services.AddInfrastructure(Configuration);
            // JSON Serializer
            services.AddSingleton<IEventSerializer>(new JsonEventSerializer(new[]
            {
                typeof(OrderCreated).Assembly,
                typeof(OrderItemsUpdated).Assembly,
                typeof(OrderConfirmed).Assembly,
            }));
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
            // Mediator handlers
            services.AddMediatR(typeof(OrderById.Handler));
            services.AddMediatR(typeof(CreateOrder.Handler));
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
