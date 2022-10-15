using Microsoft.AspNetCore.Mvc;
using MusicCollection.Application.Common.Interfaces;

namespace MusicCollection.API.Controllers;

public class BaseController : ControllerBase
{
    public IActionResult CreateResponse(IApiResult result)
    {
        if (result.GetType().IsGenericType && result.GetType().GetGenericTypeDefinition() == typeof(ApiResult<>))
        {
            return Ok(result.GetPayload());
        }

        return result switch
        {
            NoContentApiResult => NoContent(),
            ResourceNotFoundApiResult => NotFound(),
            ResourceAlreadyExistApiResult => BadRequest(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}