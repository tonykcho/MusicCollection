using MusicCollection.Domain.Common.Abstractions;
using MusicCollection.Domain.Common.Interfaces;

namespace MusicCollection.Application.Common.Interfaces;

public interface IRepository<T> where T: BaseEntity, IAggregateRoot
{
    public T GetByID(int id);

    public T GetByGuid(Guid guid);
}