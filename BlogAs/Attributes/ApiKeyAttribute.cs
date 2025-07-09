using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogAs.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKeyName, out var extratedApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "Unauthorized"
            };
            return;
        }

        if (!Configuration.ApiKey.Equals(extratedApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "Unauthorized"
            };
            return;
        }

        await next();
    }
}