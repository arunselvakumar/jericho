namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;

    interface IFavoritesService
    {
        Task GetAllFavoritesDirectoryAsync();

        Task SaveFavoritesDirectoryAsync();

        Task DeleteFavoritesDirectoryAsync();

        Task GetPostsFromFavoritesDirectoryAsync(string directoryId);

        Task AddPostToFavoritesDirectoryAsync();

        Task DeletePostFromFavoritesDirectoryAsync();
    }   
}