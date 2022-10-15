namespace MusicCollection.Application.Common.Interfaces;

public class NoContentApiResult : IApiResult
{
}

public class ApiResult<T> : IApiResult where T : class
{
    private readonly T _payload;

    public ApiResult(T payload)
    {
        _payload = payload;
    }

    public object GetPayload()
    {
        return _payload;
    }
}

public class ResourceNotFoundApiResult : IApiResult
{
}

public class ResourceAlreadyExistApiResult : IApiResult
{
}

public interface IApiResult
{
    public object? GetPayload() => null;
}

public static class ApiErrors
{
    public static ResourceAlreadyExistApiResult AlbumAlreadyExist => new ResourceAlreadyExistApiResult();
}