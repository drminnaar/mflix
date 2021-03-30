using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace MFlix.HttpApi.Controllers
{
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public sealed class RootController : ApiControllerBase
    {
        [HttpGet(Name = nameof(Get))]
        public IActionResult Get()
        {
            var links = new ResourceLinkCollection(GetRootResourceLinks())
                .AddRange(GetMovieResourceLinks());

            return Ok(links);
        }

        [HttpOptions(Name = nameof(GetRootOptions))]
        public IActionResult GetRootOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            return Ok();
        }

        private IReadOnlyCollection<ResourceLink> GetRootResourceLinks() => new List<ResourceLink>
        {
            new (Href(nameof(RootController.Get)), "self", HttpMethod.Get),
            new (Href(nameof(RootController.GetRootOptions)), "options", HttpMethod.Options)
        };

        private IReadOnlyCollection<ResourceLink> GetMovieResourceLinks() => new List<ResourceLink>
        {
            new (Href(nameof(MoviesController.GetMovieById), new { movieId = "{movieId}" }), "movie-get", HttpMethod.Get),
            new (Href(nameof(MoviesController.ListMovies)), "movie-list", HttpMethod.Get),
            new (Href(nameof(MoviesController.GetMovieOptions)), "movie-options", HttpMethod.Options),
            new (Href(nameof(MoviesController.SaveMovie)), "movie-save", HttpMethod.Post),
            new (Href(nameof(MoviesController.SaveImdbRating), new { movieId = "{movieId}" }), "imdb-save", HttpMethod.Post)
        };

        private string Href(string routeName, object? values) =>
            HttpUtility.UrlDecode(Url.Link(routeName, values) ?? "");

        private string Href(string routeName) =>
            Url.Link(routeName, new { }) ?? string.Empty;
    }
}
