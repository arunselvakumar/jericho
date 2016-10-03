namespace Jericho.Config
{
    using System;

    using AutoMapper;

    using Extensions;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs;
    using Jericho.Models.v1.DTOs.User;
    using Jericho.Models.v1.Entities;
    using MongoDB.Bson;

    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            this.ConfigurePostMappers();
            this.ConfigureUserMappers();
        }

        private void ConfigurePostMappers()
        {
            this.CreateMap<PostDto, PostEntity>()
                .ForMember(postEntity => postEntity.Id, opt => opt.MapFrom(postDto => ObjectId.Parse(postDto.Id)));

            this.CreateMap<PostEntity, PostDto>()
                .ForMember(postDto => postDto.Id, opt => opt.MapFrom(postEntity => postEntity.Id.ToString()));
        }

        private void ConfigureUserMappers()
        {
            this.CreateMap<ApplicationUser, UserDto>()
                .ForMember(userDto => userDto.EMail, opt => opt.MapFrom(appUser => appUser.Email.NormalizedValue.ToLower()));
        }
    }
}