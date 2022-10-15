using Microsoft.EntityFrameworkCore;
using MusicCollection.Application.Common.Interfaces;
using MusicCollection.DataAccess.DbContexts;
using MusicCollection.Domain.Entities;

namespace MusicCollection.DataAccess.Repositories;

public class AlbumRepository : BaseRepository<Album>, IAlbumRepository
{
    public AlbumRepository(MusicCollectionDbContext context) : base(context)
    {
    }

    public async Task<Album?> GetAlbumByName(string albumName, CancellationToken cancellationToken)
    {
        return await Context.Set<Album>().SingleOrDefaultAsync(album => album.AlbumName == albumName, cancellationToken);
    }
}