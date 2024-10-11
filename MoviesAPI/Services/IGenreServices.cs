namespace MoviesAPI.Services
{
    public interface IGenreServices
    {
        public Task<IEnumerable<Genre>> GetAll();
        public Task<Genre> Add(Genre genre);
        public Task<Genre> GetById(byte id);
        public Genre Update(Genre genre);
        public Genre Delete(Genre genre);
        Task<bool> IsvalidGenre(byte id);
    }
}
