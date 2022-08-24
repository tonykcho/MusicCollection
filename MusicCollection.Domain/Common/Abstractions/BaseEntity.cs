namespace MusicCollection.Domain.Common.Abstractions;

public class BaseEntity
{
    public int Id { get; set; }

    public Guid Guid { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }
}