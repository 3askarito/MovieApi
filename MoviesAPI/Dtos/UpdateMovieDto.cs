namespace MoviesAPI.Dtos
{
    public class UpdateMovieDto : MovieDto
    {
        public IFormFile? Poster { get; set; }
    }
}
