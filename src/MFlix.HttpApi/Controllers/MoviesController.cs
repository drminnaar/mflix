using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MFlix.HttpApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MFlix.HttpApi.Controllers
{
    [Route("movies")]
    public sealed class MoviesController : ApiControllerBase
    {
        private readonly Services.MovieService.MovieServiceClient _movieService;
        private readonly IMapper _mapper;

        public MoviesController(Services.MovieService.MovieServiceClient movieService, IMapper mapper)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{movieId}", Name = nameof(GetMovieById))]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMovieById([FromRoute] string movieId)
        {
            var response = await _movieService.GetMovieByIdAsync(new Services.GetMovieByIdRequest
            {
                MovieId = movieId
            });

            if (response?.Movie is null)
                return NotFound();

            return Ok(_mapper.Map<Movie>(response.Movie));
        }

        [HttpGet(Name = nameof(ListMovies))]
        [ProducesResponseType(typeof(Movie[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListMovies([FromQuery] MovieOptions options)
        {
            var response = await _movieService.GetMovieListAsync(new Services.GetMovieListRequest
            {
                Options = _mapper.Map<Services.MovieOptions>(options)
            });

            return Ok(_mapper.Map<IEnumerable<Movie>>(response.Movies));
        }

        [HttpPost(Name = nameof(SaveMovie))]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveMovie([FromBody] MovieForSave movieForSave)
        {
            var saveRequest = new Services.SaveMovieRequest
            {
                Movie = _mapper.Map<Services.MovieForSave>(movieForSave)
            };
            var saveResponse = await _movieService.SaveMovieAsync(saveRequest);

            var getRequest = new Services.GetMovieByIdRequest
            {
                MovieId = saveResponse.MovieId
            };
            var getResponse = await _movieService.GetMovieByIdAsync(getRequest);
            var movie = _mapper.Map<Movie>(getResponse.Movie);

            return CreatedAtAction(
                actionName: nameof(GetMovieById),
                routeValues: new { movieId = movie.Id },
                value: movie);
        }

        [HttpPost("{movieId}/imdb", Name = nameof(SaveImdbRating))]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveImdbRating([FromRoute] string movieId, [FromBody] ImdbForSave imdbForSave)
        {
            var request = new Services.SaveImdbRatingRequest
            {
                Imdb = _mapper.Map<Services.Imdb>(imdbForSave),
                MovieId = movieId
            };

            await _movieService.SaveImdbRatingAsync(request);

            return NoContent();
        }

        [HttpOptions(Name = nameof(GetMovieOptions))]
        public IActionResult GetMovieOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            return Ok();
        }
    }
}
