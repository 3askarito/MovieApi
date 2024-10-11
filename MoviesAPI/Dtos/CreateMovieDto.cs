namespace MoviesAPI.Dtos
{
    public class CreateMovieDto : MovieDto
    {
        public IFormFile Poster { get; set; }
    }
}
