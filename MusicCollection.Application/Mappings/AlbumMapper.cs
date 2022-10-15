using MusicCollection.Application.Dtos;
using MusicCollection.Domain.Entities;

namespace MusicCollection.Application.Mappings;

public class AlbumMapper
{
    public static AlbumReadDto From(Album album)
    {
        return new AlbumReadDto()
        {
            AlbumName = album.AlbumName,
            Circle = album.Circle,
            ReleaseDate = album.ReleaseDate
        };
    }
}