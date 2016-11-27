namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;
    using Jericho.Models.v1.Entities;

    public interface IFavoritesService
    {
        Task GetAllFavoritesDirectoryAsync();

        Task SaveFavoritesDirectoryAsync(FavoriteEntity entity);

        Task DeleteFavoritesDirectoryAsync();

        Task GetPostsFromFavoritesDirectoryAsync(string directoryId);

        Task AddPostToFavoritesDirectoryAsync();

        Task DeletePostFromFavoritesDirectoryAsync();
    }   
}