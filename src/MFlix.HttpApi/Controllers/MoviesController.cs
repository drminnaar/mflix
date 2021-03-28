using System;
using System.Threading.Tasks;
using MFlix.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MFlix.HttpApi.Controllers
{
    [Route("movies")]
    public sealed class MoviesController : ApiControllerBase
    {
        private readonly MovieService.MovieServiceClient _movieService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(MovieService.MovieServiceClient movieService, ILogger<MoviesController> logger)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{movieId}", Name = nameof(GetMovieById))]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMovieById([FromRoute] string movieId)
        {
            var response = await _movieService.GetMovieByIdAsync(new GetMovieByIdRequest
            {
                MovieId = movieId
            });

            if (response?.Movie is null)
                return NotFound();

            return Ok(response.Movie);
        }

        [HttpOptions(Name = nameof(GetMovieOptions))]
        public IActionResult GetMovieOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            return Ok();
        }
    }
}
