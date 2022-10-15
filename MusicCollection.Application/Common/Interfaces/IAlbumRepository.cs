using MusicCollection.Domain.Entities;

namespace MusicCollection.Application.Common.Interfaces;

public interface IAlbumRepository : IRepository<Album>
{
    public Task<Album?> GetAlbumByName(string albumName, CancellationToken cancellationToken);
}