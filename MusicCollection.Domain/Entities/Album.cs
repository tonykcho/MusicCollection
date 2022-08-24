using MusicCollection.Domain.Common.Abstractions;
using MusicCollection.Domain.Common.Interfaces;

namespace MusicCollection.Domain.Entities;

public class Album : BaseEntity, IAggregateRoot
{
    public string AlbumName { get; set; } = string.Empty;
    public string Circle { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
}