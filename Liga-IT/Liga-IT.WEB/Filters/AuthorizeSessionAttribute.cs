using Microsoft.AspNetCore.Mvc.Filters;

namespace Liga_IT.WEB.Filters;

public class AuthorizeSessionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var isAuthenticated = bool.Parse(context.HttpContext.Session.GetString("IsAuthenticated") ?? "false");
        if (!isAuthenticated)
        {
            context.HttpContext.Response.Redirect("/Auth/Index");
        }
        base.OnActionExecuting(context);
    }
}

