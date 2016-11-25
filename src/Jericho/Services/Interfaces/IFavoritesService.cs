namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IFavoritesService
    {
        Task GetAllFavoritesDirectoryAsync();

        Task SaveFavoritesDirectoryAsync();

        Task DeleteFavoritesDirectoryAsync();

        Task GetPostsFromFavoritesDirectoryAsync(string directoryId);

        Task AddPostToFavoritesDirectoryAsync();

        Task DeletePostFromFavoritesDirectoryAsync();
    }   
}