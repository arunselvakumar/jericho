namespace Jericho.Config
{
    using System;

    using AutoMapper;

    using Extensions;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs;
    using Jericho.Models.v1.DTOs.User;
    using Jericho.Models.v1.Entities;

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
                .ForMember(postEntity => postEntity.UpVotes, opt => opt.UseValue(0))
                .ForMember(postEntity => postEntity.DownVotes, opt => opt.UseValue(0))
                .ForMember(postEntity => postEntity.Version, opt => opt.UseValue(DateTime.Now))
                .ForMember(
                    postEntity => postEntity.Url,
                    opt => opt.MapFrom(
                    postDto => $"{postDto.Title.Trim().Replace(' ', '_').ToLower()}_{DateTime.UtcNow.ToTimeStamp()}"));

            this.CreateMap<PostEntity, PostDto>();
        }

        private void ConfigureUserMappers()
        {
            this.CreateMap<ApplicationUser, UserDto>()
                .ForMember(userDto => userDto.EMail, opt => opt.MapFrom(appUser => appUser.Email.NormalizedValue.ToLower()));
        }
    }
}