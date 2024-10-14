namespace MoviesAPI.Services
{
    public interface IMovieServices
    {
        Task<IEnumerable<Movie>> GetAll(int pageNumber = 1, int pageSize = 10, byte genreId = 0);
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(Movie movie);

    }
}
