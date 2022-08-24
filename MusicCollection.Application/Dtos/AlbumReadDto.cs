namespace MusicCollection.Application.Dtos;

public class AlbumReadDto
{
    public string AlbumName { get; set; } = string.Empty;
    public string Circle { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
}