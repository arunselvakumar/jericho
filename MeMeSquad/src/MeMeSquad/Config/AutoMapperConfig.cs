using System;
using MeMeSquad.Models.DTOs;
using MeMeSquad.Models.Entities;

namespace MeMeSquad.Config
{
    using AutoMapper;

    public class AutoMapperConfig : Profile
    {
        #region Constructor

        public AutoMapperConfig()
        {
            this.ConfigurePostMappers();
            this.ConfigureUserMappers();
        }
        #endregion

        #region Private Methods

        private void ConfigurePostMappers()
        {
            CreateMap<PostDto, PostEntity>()
                .ForMember(postEntity => postEntity.Id, opt => opt.UseValue(new Guid()))
                .ForMember(postEntity => postEntity.UpVotes, opt => opt.UseValue(0))
                .ForMember(postEntity => postEntity.DownVotes, opt => opt.UseValue(0))
                .ForMember(postEntity => postEntity.Version, opt => opt.UseValue(DateTime.Now));
        }

        private void ConfigureUserMappers()
        {
            
        }
        #endregion
    }
}