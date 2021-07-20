using System;
using Amazon.DynamoDBv2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.API.DTOs;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderAPI.Infrastructure.Core;
using OrderApi.Infrastructure.EventStore;
using OrderApi.Infrastructure.Persistence;
using OrderApi.Infrastructure.Repositores;

namespace OrderApi.API.Extensions
{
    public static class InfrastructureRegistry
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration config)
        {
            // EntityEvent DbContext
            services.AddDbContext<EntityEventContext>(opt =>
                opt.UseSqlite(config.GetConnectionString("DefaultConnection")
                ), ServiceLifetime.Singleton);
            // DynamoDB
            var awsOptions = config.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonDynamoDB>();

            services.AddSingleton<IEventsRepository<Order,Guid>, EventsRepository<Order,Guid>>();
            services.AddSingleton<IReadStoreRepository<Order, Guid>, ReadStoreRepository<Order, Guid>>();
            services.AddSingleton<IEventsService<Order,Guid>, EventsService<Order,Guid>>();

            return services;
        }
    }
}