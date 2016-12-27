﻿namespace Jericho.Configuration
{
    using System;

    using AutoMapper;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs;
    using Jericho.Models.v1.DTOs.Favorite;
    using Jericho.Models.v1.DTOs.User;
    using Jericho.Models.v1.Entities;
    using Jericho.Models.v1.Entities.Enums;
    using Jericho.Providers.ServiceResultProvider;

    using Microsoft.AspNetCore.Identity;

    using MongoDB.Bson;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {            
            this.ConfigureUserMappers();
            this.ConfigurePostMappers();
            this.ConfigureFavoriteMappers();
        }

        private void ConfigureUserMappers()
        {
            this.CreateMap<IdentityError, Error>();

            this.CreateMap<SaveUserRequestDto, ApplicationUser>()
                .ConstructUsing(dto => new ApplicationUser(dto.UserName, dto.EMail))
                .ForMember(user => user.Email, opt => opt.Ignore());

            this.CreateMap<ApplicationUser, GetUserResponseDto>()
                .ForMember(userDto => userDto.EMail, opt => opt.MapFrom(appUser => appUser.Email.NormalizedValue.ToLower()));
        }

        private void ConfigurePostMappers()
        {
            this.CreateMap<UpdatePostDto, PostEntity>()
                .ForMember(postEntity => postEntity.Id, opt => opt.MapFrom(postDto => string.IsNullOrEmpty(postDto.Id) ? ObjectId.Empty : ObjectId.Parse(postDto.Id)));

            this.CreateMap<PostEntity, UpdatePostDto>()
                .ForMember(postDto => postDto.Id, opt => opt.MapFrom(postEntity => postEntity.Id.ToString()));
        }

        private void ConfigureFavoriteMappers()
        {
            this.CreateMap<SaveFavoriteDirectoryDto, FavoriteEntity>()
                .ForMember(favoriteEntity => favoriteEntity.CreatedOn, opt => opt.MapFrom(x => DateTime.UtcNow))
                .ForMember(favoriteEntity => favoriteEntity.FavoriteType, opt => opt.MapFrom(x => FavoriteTypeEnum.Directory));

            this.CreateMap<SaveFavoritePostDto, FavoriteEntity>()
                .ForMember(favoriteEntity => favoriteEntity.CreatedOn, opt => opt.MapFrom(x => DateTime.UtcNow))
                .ForMember(favoriteEntity => favoriteEntity.FavoriteType, opt => opt.MapFrom(x => FavoriteTypeEnum.Post));
        }
    }
}