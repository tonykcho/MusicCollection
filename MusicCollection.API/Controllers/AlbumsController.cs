using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace MusicCollection.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumsController : ControllerBase
{
    private readonly IOutputCacheStore _outputCacheStore;

    public AlbumsController(IOutputCacheStore outputCacheStore)
    {
        _outputCacheStore = outputCacheStore;
    }

    [HttpGet]
    public IActionResult GetAlbums(CancellationToken cancellationToken = default)
    {
        _outputCacheStore.EvictByTagAsync("1", cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Route("{id}")]
    [OutputCache(PolicyName = "ById")]
    public IActionResult GetAlbum(int id, CancellationToken cancellationToken = default)
    {
        return Ok("This is a message");
    }
}