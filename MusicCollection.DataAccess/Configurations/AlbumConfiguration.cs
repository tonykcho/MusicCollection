using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCollection.Domain.Entities;

namespace MusicCollection.DataAccess.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder
            .ToTable("Albums");

        builder
            .HasKey(album => album.Id);

        builder
            .Property(album => album.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasIndex(album => album.Id);
    }
}