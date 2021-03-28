using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace MFlix.HttpApi.Controllers
{
    [ApiController]
    [Produces(
        MediaTypeNames.Application.Json,
        MediaTypeNames.Application.Xml,
        "application/problem+xml",
        "application/problem+json")]
    [Consumes(
        MediaTypeNames.Application.Json,
        MediaTypeNames.Application.Xml)]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ApiControllerBase() : base()
        {
        }
    }
}
