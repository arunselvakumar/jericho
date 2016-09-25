using Jericho.Helpers;
using Jericho.Helpers.Interfaces;
using MeMeSquad.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MeMeSquad.Extensions
{
    using System;
    using AutoMapper;

    using MeMeSquad.Config;
    using MeMeSquad.Services;
    using MeMeSquad.Services.Interfaces;
    using MeMeSquad.Validations;
    using MeMeSquad.Validations.Interfaces;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtensions
    {
        public static void AddUserService(this IServiceCollection service)
        {
            if(service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IUserService, UserService>();
        }

        public static void AddPostService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IPostService, PostService>();
        }

        public static void AddCreateUserValidationService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<ICreateUserValidationService, CreateUserValidationService>();
        }

        public static void AddIdentity(this IServiceCollection service, IConfigurationRoot configuration)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IUserStore<MongoIdentityUser>>(provider =>
            {
                var options = provider.GetService<IOptions<MongoDbConfig>>();
                var client = new MongoClient(options.Value.ConnectionString);
                var database = client.GetDatabase(options.Value.DatabaseName);
                var loggerFactory = provider.GetService<ILoggerFactory>();

                return new MongoUserStore<MongoIdentityUser>(database, loggerFactory);
            });
        }

        public static void AddAutoMapper(this IServiceCollection service, MapperConfiguration mapperConfiguration)
        {
            mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperConfig());
            });

            service.AddSingleton(sp => mapperConfiguration.CreateMapper());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public static void AddMongoDbInstance(this IServiceCollection service)
        {
            if(service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IMongoHelper, MongoHelper>();
        }
    }
}