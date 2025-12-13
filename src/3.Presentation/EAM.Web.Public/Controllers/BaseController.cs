using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using EAM.Web.Public.Configuration;

namespace EAM.Web.Public.Controllers;

public class BaseController : Controller
{
    protected readonly AppSettings AppSettings;

    public BaseController(IOptions<AppSettings> appSettings)
    {
        AppSettings = appSettings.Value;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.AppSettings = AppSettings;
        base.OnActionExecuting(context);
    }
}
