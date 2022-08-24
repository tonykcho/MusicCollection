using MusicCollection.Application.Common.Interfaces;
using MusicCollection.Domain.Common.Abstractions;
using MusicCollection.Domain.Common.Interfaces;

namespace MusicCollection.DataAccess.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity, IAggregateRoot
{
    public T GetByGuid(Guid guid)
    {
        throw new NotImplementedException();
    }

    public T GetByID(int id)
    {
        throw new NotImplementedException();
    }
}