
namespace MoviesAPI.Services
{
    public class GenreServices(ApplicationDbContext dbContext) : IGenreServices
    {
        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await dbContext.Genres.OrderBy(g => g.Name).ToListAsync();
        }
        public async Task<Genre> Add(Genre genre)
        {
            await dbContext.AddAsync(genre);
            dbContext.SaveChanges();
            return genre;
        }

        public async Task<Genre> GetById(byte id)
        {
            return await dbContext.Genres.SingleOrDefaultAsync(g => g.Id == id);
        }
        public Genre Update(Genre genre)
        {
            dbContext.Update(genre);
            dbContext.SaveChanges();
            return genre;
        }
        public Genre Delete(Genre genre)
        {
            dbContext.Remove(genre);
            dbContext.SaveChanges();
            return genre;
        }

        public async Task<bool> IsvalidGenre(byte id)
        {
            return await dbContext.Genres.AnyAsync(g => g.Id == id);
        }

       
    }
}
