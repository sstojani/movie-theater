using eTicket.Data.Base;
using eTicket.Data.ViewModels;
using eTicket.Models;

namespace eTicket.Data.Services
{
    public interface IMoviesService : IEntityBaseRepository<Movie>
    {
        Task<Movie> GetMoviesByIdAsync(int id);

        Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues();

        Task AddNewMovieAsync(NewMovieVM data);

        Task UpdateMovieAsync(NewMovieVM data);

        Task DeleteMovieAsync(int id);

    }
}
