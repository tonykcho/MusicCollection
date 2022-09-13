using Microsoft.AspNetCore.Mvc.Filters;

namespace MusicCollection.API.Filters;

public class TestFilter : IResourceFilter, IOrderedFilter
{
    public int Order => throw new NotImplementedException();

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        throw new NotImplementedException();
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        throw new NotImplementedException();
    }
}