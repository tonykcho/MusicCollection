using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using MusicCollection.Application.Commands.Albums;
using MusicCollection.Application.Common.Interfaces;
using MusicCollection.Application.Queries.Albums;

namespace MusicCollection.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IOutputCacheStore _outputCacheStore;
    private readonly IDataProtectionProvider _dataProtectionProvider;

    public AlbumsController(IOutputCacheStore outputCacheStore, IDataProtectionProvider dataProtectionProvider, IMediator mediator)
    {
        _outputCacheStore = outputCacheStore;
        _dataProtectionProvider = dataProtectionProvider;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAlbumList([FromRoute] GetAlbumListQuery query, CancellationToken cancellationToken = default)
    {
        // _outputCacheStore.EvictByTagAsync("1", cancellationToken);

        return CreateResponse(await _mediator.Send(query, cancellationToken));
    }

    [HttpGet("{Guid}")]
    // [OutputCache(PolicyName = "ById")]
    public async Task<IActionResult> GetAlbum([FromRoute] GetAlbumByGuidQuery query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return CreateResponse(await _mediator.Send(query, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAlbum([FromBody] CreateAlbumCommand command, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return CreateResponse(await _mediator.Send(command, cancellationToken));
    }
}