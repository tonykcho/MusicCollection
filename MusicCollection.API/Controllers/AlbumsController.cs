using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.DataProtection;

namespace MusicCollection.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumsController : ControllerBase
{
    private readonly IOutputCacheStore _outputCacheStore;
    private readonly IDataProtectionProvider _dataProtectionProvider;

    public AlbumsController(IOutputCacheStore outputCacheStore, IDataProtectionProvider dataProtectionProvider)
    {
        _outputCacheStore = outputCacheStore;
        _dataProtectionProvider = dataProtectionProvider;
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