using MusicCollection.Domain.Common.Abstractions;
using MusicCollection.Domain.Common.Interfaces;

namespace MusicCollection.Application.Common.Interfaces;

public interface IRepository<T> where T: BaseEntity, IAggregateRoot
{
    public IUnitOfWork UnitOfWork { get; }

    public Task<ICollection<T>> ListAsync(CancellationToken cancellationToken);

    public Task<ICollection<T>> ListAsync(IQueryable<T> query, CancellationToken cancellationToken);

    public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);

    public Task<T?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken);

    public Task AddAsync(T entity, CancellationToken cancellationToken);

    public IQueryable<T> GetQuery();
    
    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}