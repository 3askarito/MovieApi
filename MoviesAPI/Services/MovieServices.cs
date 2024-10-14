
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Services
{
    public class MovieServices(ApplicationDbContext dbContext) : IMovieServices
    {
        public async Task<Movie> Add(Movie movie)
        {
            await dbContext.AddAsync(movie);
            dbContext.SaveChanges();
            return movie;
        }

        public Movie Delete(Movie movie)
        {
            dbContext.Remove(movie);
            dbContext.SaveChanges();
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(int pageNumber  = 1, int pageSize = 10, byte genreId = 0)
        {
            return await dbContext.Movies.Where(m=> m.GenreId == genreId || genreId == 0).OrderByDescending(m => m.Rate).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(m => m.Genre).ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await dbContext.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
        }

        public Movie Update(Movie movie)
        {
            dbContext.Update(movie);
            dbContext.SaveChanges();
            return movie;
        }
    }
}
