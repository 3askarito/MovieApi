using Microsoft.AspNetCore.Authorization;

namespace MoviesAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController(IGenreServices genreServices) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await genreServices.GetAll();
            return Ok(genres);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(byte id)
        {
            var genre = await genreServices.GetById(id);
            return Ok(genre);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto genreDto)
        {
            var genre = new Genre
            {
                Name = genreDto.Name
            };
            await genreServices.Add(genre);
            return Ok(genre);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdaetAsync(byte id, [FromBody] GenreDto dto)
        {
            var genre = await genreServices.GetById(id);
            if (genre is null)
                return NotFound($"There is no genre with ID : {id}");
            genre.Name = dto.Name;
            genreServices.Update(genre);
            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DelteAsync(byte id)
        {
            var genre = await genreServices.GetById(id);
            if (genre is null)
                return NotFound($"There is no genre with ID : {id}");
            genreServices.Delete(genre);
            return Ok(genre);
        }
    }
}
