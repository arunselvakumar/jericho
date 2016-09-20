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

        public static void AddAutoMapper(this IServiceCollection service, MapperConfiguration mapperConfiguration)
        {
            mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperConfig());
            });

            service.AddSingleton(sp => mapperConfiguration.CreateMapper());
        }
    }
}