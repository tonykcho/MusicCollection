using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCollection.Application.Common.Interfaces;
using MusicCollection.Application.Dtos;
using MusicCollection.Application.Mappings;
using MusicCollection.Domain.Entities;

namespace MusicCollection.Application.Queries.Albums;

public class GetAlbumByGuidQuery : IRequest<IApiResult>
{
    [Required]
    [FromRoute]
    public Guid Guid { get; set; } = Guid.Empty;
}

public class GetAlbumByGuidHandler : IRequestHandler<GetAlbumByGuidQuery, IApiResult>
{
    private readonly IAlbumRepository _albumRepository;

    public GetAlbumByGuidHandler(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }
    
    public async Task<IApiResult> Handle(GetAlbumByGuidQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Album? album = await _albumRepository.GetByGuidAsync(request.Guid, cancellationToken);

        if (album == null)
        {
            return new ResourceNotFoundApiResult();
        }

        return new ApiResult<AlbumReadDto>(AlbumMapper.From(album));
    }
}