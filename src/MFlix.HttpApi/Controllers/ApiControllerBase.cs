using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace MFlix.HttpApi.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ApiControllerBase() : base()
        {
        }
    }
}
