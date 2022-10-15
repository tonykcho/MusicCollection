using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicCollection.Application.Common.Interfaces;
using MusicCollection.Domain.Common.Abstractions;
using MusicCollection.Domain.Entities;

namespace MusicCollection.DataAccess.DbContexts;

public class MusicCollectionDbContext : DbContext, IUnitOfWork
{
    private readonly IConfiguration _configuration;

    public MusicCollectionDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql(_configuration.GetConnectionString("MusicCollection"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var transaction = await Database.BeginTransactionAsync(cancellationToken))
        {
            await action();

            await transaction.CommitAsync(cancellationToken);
        }
    }

    public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var transaction = await Database.BeginTransactionAsync(cancellationToken))
        {
            TResult result = await action();

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Guid = Guid.NewGuid();
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastUpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        // var events = ChangeTracker.Entries<IHasDomainEvent>()
        //     .Select(x => x.Entity.DomainEvents)
        //     .SelectMany(x => x)
        //     .ToArray();

        var result = await base.SaveChangesAsync(cancellationToken);

        // await DispatchEvents(events, cancellationToken);

        return result;
    }

    // private async Task DispatchEvents<T>(T[] events, CancellationToken cancellationToken) where T : DomainEvent
    // {
    //     cancellationToken.ThrowIfCancellationRequested();

    //     foreach (var domainEvent in events)
    //     {
    //         await _messageBusClient.PublishDomainEventAsync(domainEvent, cancellationToken);
    //     }
    // }
}