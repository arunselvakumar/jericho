namespace Jericho.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Jericho.Models.v1.Entities;
    using Jericho.Providers.ServiceResultProvider;

    public interface IFavoritesService
    {
        Task<ServiceResult<IEnumerable<FavoriteEntity>>> GetAllFavoritesDirectoryAsync(string userId);

        Task<ServiceResult<FavoriteEntity>> SaveFavoritesDirectoryAsync(FavoriteEntity entity, string userId);

        Task<ServiceResult<object>> DeleteFavoritesDirectoryAsync(string id);

        Task<ServiceResult<IEnumerable<FavoriteEntity>>> GetPostsFromFavoritesDirectoryAsync(string directoryId);

        Task<ServiceResult<FavoriteEntity>> AddPostToFavoritesDirectoryAsync(string id, FavoriteEntity entity);

    }   
}