using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using MusicCollection.Application.Common.Interfaces;
using MusicCollection.Application.Dtos;
using MusicCollection.Application.Mappings;
using MusicCollection.Domain.Entities;

namespace MusicCollection.Application.Commands.Albums;

public class CreateAlbumCommand : IRequest<IApiResult>
{
    [Required]
    public string AlbumName { get; set; } = string.Empty;

    [Required]
    public string Circle { get; set; } = string.Empty;

    [Required]
    public DateTime ReleaseDate { get; set; }
}

public class CreateAlbumHandler : IRequestHandler<CreateAlbumCommand, IApiResult>
{
    private readonly IAlbumRepository _albumRepository;

    public CreateAlbumHandler(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }
    
    public async Task<IApiResult> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _albumRepository.GetAlbumByName(request.AlbumName, cancellationToken) != null)
        {
            return ApiErrors.AlbumAlreadyExist;
        }

        Album album = new Album()
        {
            AlbumName = request.AlbumName,
            Circle = request.Circle,
            ReleaseDate = request.ReleaseDate
        };

        await _albumRepository.AddAsync(album, cancellationToken);

        await _albumRepository.SaveChangesAsync(cancellationToken);

        return new ApiResult<AlbumReadDto>(AlbumMapper.From(album));
    }
}