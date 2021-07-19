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
                opt.UseSqlite(config.GetConnectionString("DefaultConnection")));
            // ReadState DB
            var awsOptions = config.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonDynamoDB>();

            services.AddEventsService<Order, Guid>(config);
            services.AddEventsRepository<Order, Guid>(config);
            services.AddReadRepository<Order, Guid>(config);

            return services;
        }

        private static IServiceCollection AddEventsService<TA, TK>(this IServiceCollection services,
            IConfiguration config)
            where TA : class, IAggregateRoot<TK>
        {
            return services.AddSingleton<IEventsService<TA, TK>>(ctx =>
            {
                // TODO: add events producer
                // var eventsProducer = ctx.GetRequiredService<IEventProducer<TA, TK>>();
                var writeRepo = ctx.GetRequiredService<IEventsRepository<TA, TK>>();
                var readRepo = ctx.GetRequiredService<IReadStoreRepository<TA, TK>>();

                return new EventsService<TA, TK>(writeRepo, readRepo);
            });
        }

        private static IServiceCollection AddEventsRepository<TA, TK>(this IServiceCollection services,
            IConfiguration config)
            where TA : class, IAggregateRoot<TK>
        {
            return services.AddSingleton<IEventsRepository<TA, TK>>(ctx =>
            {
                // TODO: Is there a better way to get the DbContext in a Singleton? 
                var sp = services.BuildServiceProvider();
                var dbContext = sp.GetRequiredService<EntityEventContext>();
                var eventDeserializer = ctx.GetRequiredService<IEventSerializer>();

                return new EventsRepository<TA, TK>(dbContext, eventDeserializer);
            });
        }
        
        private static IServiceCollection AddReadRepository<TA, TK>(this IServiceCollection services,
            IConfiguration config)
            where TA : class, IAggregateRoot<TK>
        {
            return services.AddSingleton<IReadStoreRepository<TA, TK>>(ctx =>
            {
                var amazonDynamoDb = ctx.GetRequiredService<IAmazonDynamoDB>();

                return new ReadStoreRepository<TA, TK>(amazonDynamoDb);
            });
        }
    }
}