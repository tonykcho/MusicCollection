using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCollection.Application.Common.Interfaces;
using MusicCollection.Application.Dtos;
using MusicCollection.Application.Mappings;
using MusicCollection.Domain.Entities;

namespace MusicCollection.Application.Queries.Albums;

public class GetAlbumListQuery : IRequest<IApiResult>
{
    [FromQuery]
    public string? Circle { get; set; } = null;
}

public class GetAlbumListHandler : IRequestHandler<GetAlbumListQuery, IApiResult>
{
    private readonly IAlbumRepository _albumRepository;

    public GetAlbumListHandler(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }

    public async Task<IApiResult> Handle(GetAlbumListQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<Album> query = _albumRepository.GetQuery();

        if (request.Circle is not null)
        {
            query = query.Where(album => album.Circle == request.Circle);
        }

        ICollection<Album> albums = await _albumRepository.ListAsync(query, cancellationToken);

        ICollection<AlbumReadDto> albumReadDtos = albums.Select(AlbumMapper.From).ToList();

        return new ApiResult<ICollection<AlbumReadDto>>(albumReadDtos);
    }
}