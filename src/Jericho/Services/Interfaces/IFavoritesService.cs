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

        Task GetPostsFromFavoritesDirectoryAsync(string directoryId);

        Task AddPostToFavoritesDirectoryAsync();

        Task DeletePostFromFavoritesDirectoryAsync();
    }   
}