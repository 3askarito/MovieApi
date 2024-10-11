using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using MoviesAPI.Data;

namespace MoviesAPI.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController(IMovieServices movieServices, IGenreServices genreServices, IMapper mapper) : ControllerBase
    {
        private new List<string> allowedExtensions = new List<string>() { ".jpg", ".png", ".jpeg"};
        private int allowedMaxFileSize = 1048576;
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movie = await movieServices.GetAll();
            var data = mapper.Map<IEnumerable<Movie>>(movie);
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await movieServices.GetById(id);
            if (movie is null)
                return NotFound("That Movie Not Found!");
            var dto = mapper.Map<MovieDetailsDto>(movie);
            return Ok(dto);
        }
        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreid)
        {
            var movie = await movieServices.GetAll(genreid);
            var data = mapper.Map<IEnumerable<Movie>>(movie);
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]CreateMovieDto dto)
        {
            if (!allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png , .jpg and .jpeg are allowed");
            if (dto.Poster.Length > allowedMaxFileSize)
                return BadRequest("The MaxSize Of the Poster Is 1MB");
            if(!await genreServices.IsvalidGenre(dto.GenreId))
                return BadRequest("The Genre Is Not Found");
            using var stream = new MemoryStream();
            await dto.Poster.CopyToAsync(stream);

            var movie = mapper.Map<Movie>(dto);
            movie.Poster = stream.ToArray();

            await movieServices.Add(movie);
            return Ok(movie);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm]UpdateMovieDto dto)
        {
            var movie = await movieServices.GetById(id);
            if (movie is null)
                return NotFound($"There is no movie with ID : {id}");
            if (!await genreServices.IsvalidGenre(dto.GenreId))
                return BadRequest("The Genre Is Not Found");
            if(dto.Poster != null)
            {
                if (!allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png , .jpg and .jpeg are allowed");
                if (dto.Poster.Length > allowedMaxFileSize)
                    return BadRequest("The MaxSize Of the Poster Is 1MB");

                using var stream = new MemoryStream();
                await dto.Poster.CopyToAsync(stream);

                movie.Poster = stream.ToArray();
            }
            movie.Title = dto.Title;
            movie.Storeline = dto.Storeline;
            movie.Rate = dto.Rate;
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
            movieServices.Update(movie);
            return Ok(movie);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await movieServices.GetById(id);
            if (movie is null)
                return NotFound($"That Movie with ID {id} is not Found!");
            movieServices.Delete(movie);
            return Ok(movie);
        }
    }
}
