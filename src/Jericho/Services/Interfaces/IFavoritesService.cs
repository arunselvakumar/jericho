namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;
    using Jericho.Models.v1.Entities;
    using Jericho.Providers.ServiceResultProvider;

    public interface IFavoritesService
    {
        Task GetAllFavoritesDirectoryAsync();

        Task<ServiceResult<FavoriteEntity>> SaveFavoritesDirectoryAsync(FavoriteEntity entity);

        Task<ServiceResult<object>> DeleteFavoritesDirectoryAsync(string id);

        Task<ServiceResult<object>> GetPostsFromFavoritesDirectoryAsync(string directoryId);

        Task<ServiceResult<FavoriteEntity>> AddPostToFavoritesDirectoryAsync(string id, FavoriteEntity entity);

        Task DeletePostFromFavoritesDirectoryAsync();
    }   
}