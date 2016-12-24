namespace Jericho.Config
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

    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {            
            this.ConfigureUserMappers();
            this.ConfigurePostMappers();
            this.ConfigureFavoriteMappers();
            this.ConfigureCommentMappers();
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
            this.CreateMap<PostDto, PostEntity>()
                .ForMember(postEntity => postEntity.Id, opt => opt.MapFrom(postDto => string.IsNullOrEmpty(postDto.Id) ? ObjectId.Empty : ObjectId.Parse(postDto.Id)));

            this.CreateMap<PostEntity, PostDto>()
                .ForMember(postDto => postDto.Id, opt => opt.MapFrom(postEntity => postEntity.Id.ToString()));
        }

        private void ConfigureFavoriteMappers()
        {
            this.CreateMap<SaveFavoriteDirectoryDto, FavoriteEntity>()
                .ForMember(favoriteEntity => favoriteEntity.CreatedOn, opt => opt.MapFrom(x => DateTime.UtcNow))
                .ForMember(favoriteEntity => favoriteEntity.FavoriteType, opt => opt.MapFrom(x => FavoriteTypeEnum.Directory));

            this.CreateMap<SaveFavoritePostDto, FavoriteEntity>()
                .ForMember(favoriteEntity => favoriteEntity.FavoriteType, opt => opt.MapFrom(x => FavoriteTypeEnum.Post));
        }

        private void ConfigureCommentMappers()
        {
            this.CreateMap<CommentDto, CommentEntity>()
                .ForMember(commentEntity => commentEntity.Type, opt => opt.MapFrom(commentDTO => GetCommentType(commentDTO.Type)))
                .ForMember(commentEntity => commentEntity.Id, opt => opt.MapFrom(commentDto => string.IsNullOrEmpty(commentDto.Id) ? ObjectId.Empty : ObjectId.Parse(commentDto.Id)))
                .ForMember(commentEntity => commentEntity.PostId, opt => opt.MapFrom(commentDto => string.IsNullOrEmpty(commentDto.PostId) ? ObjectId.Empty : ObjectId.Parse(commentDto.PostId)))
                .ForMember(commentEntity => commentEntity.ParentId, opt => opt.MapFrom(commentDto => string.IsNullOrEmpty(commentDto.ParentId) ? ObjectId.Empty : ObjectId.Parse(commentDto.ParentId)));
                

            this.CreateMap<CommentEntity, CommentDto>()
                .ForMember(commentDto => commentDto.Type, opt => opt.MapFrom(commentEntity => commentEntity.Type.ToString()))
                .ForMember(commentDto => commentDto.Id, opt => opt.MapFrom(commentEntity => commentEntity.Id.ToString()))
                .ForMember(commentDto => commentDto.PostId, opt => opt.MapFrom(commentEntity => commentEntity.PostId.ToString()))
                .ForMember(commentDto => commentDto.ParentId, opt => opt.MapFrom(commentEntity => commentEntity.ParentId.ToString()));
        }

        private CommentTypeEnum GetCommentType(string commentDTOType)
        {
            CommentTypeEnum type;
            Enum.TryParse(commentDTOType, out type);
            return type;
        }
    }
}