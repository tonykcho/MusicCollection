using Microsoft.AspNetCore.Mvc;

namespace MusicCollection.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAlbums()
    {
        return Ok();
    }

    [HttpGet]
    [Route("{AlbumId}")]
    public IActionResult GetAlbum(int AlbumId)
    {
        return Ok("This is a message");
    }
}