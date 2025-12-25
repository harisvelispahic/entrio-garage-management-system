using IoT.Application.Devices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IoT.API.Security;

public class DeviceAuthorizeAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var auth = context.HttpContext.RequestServices
            .GetRequiredService<IDeviceAuthenticator>();

        if (!context.HttpContext.Request.Headers.TryGetValue("X-Device-Key", out var key))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var device = await auth.AuthenticateAsync(key!);
        if (device == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["Device"] = device;
        await next();
    }
}
