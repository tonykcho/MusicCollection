using System.ComponentModel.DataAnnotations;
using MediatR;
using MusicCollection.Application.Common.Interfaces;
using MusicCollection.Domain.Entities;

namespace MusicCollection.Application.Commands.Albums;

public class UpdateAlbumCommand : IRequest<IApiResult>
{
    [Required]
    public Guid Guid { get; set; }
    
    [Required]
    public string AlbumName { get; set; } = string.Empty;

    [Required]
    public string Circle { get; set; } = string.Empty;

    [Required]
    public DateTime ReleaseDate { get; set; }
}

public class UpdateAlbumHandler : IRequestHandler<UpdateAlbumCommand, IApiResult>
{
    private readonly IAlbumRepository _albumRepository;

    public UpdateAlbumHandler(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }

    public async Task<IApiResult> Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Album? album = await _albumRepository.GetAlbumByName(request.AlbumName, cancellationToken);

        if (album is null)
        {
            return new ResourceNotFoundApiResult();
        }

        if (album.Guid != request.Guid)
        {
            return ApiErrors.AlbumAlreadyExist;
        }

        album.AlbumName = request.AlbumName;
        album.Circle = request.Circle;
        album.ReleaseDate = request.ReleaseDate;

        await _albumRepository.SaveChangesAsync(cancellationToken);

        return new NoContentApiResult();
    }
} 