using Microsoft.EntityFrameworkCore;
using MusicCollection.Application.Common.Interfaces;
using MusicCollection.DataAccess.DbContexts;
using MusicCollection.Domain.Common.Abstractions;
using MusicCollection.Domain.Common.Interfaces;

namespace MusicCollection.DataAccess.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity, IAggregateRoot
{
    protected readonly MusicCollectionDbContext Context;

    public IUnitOfWork UnitOfWork => Context;

    protected BaseRepository(MusicCollectionDbContext context)
    {
        Context = context;
    }

    public virtual async Task<ICollection<T>> ListAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<ICollection<T>> ListAsync(IQueryable<T> query, CancellationToken cancellationToken)
    {
        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await Context.Set<T>().SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }
    
    public virtual async Task<T?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        return await Context.Set<T>().SingleOrDefaultAsync(entity => entity.Guid == guid, cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await Context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public virtual IQueryable<T> GetQuery()
    {
        return Context.Set<T>();
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return (await Context.SaveChangesAsync(cancellationToken)) > 0;
    }
}