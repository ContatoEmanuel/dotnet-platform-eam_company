using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EAM.Web.Public.Models;
using EAM.Core.Application.Services.Interfaces;
using Microsoft.Extensions.Options;
using EAM.Web.Public.Configuration;

namespace EAM.Web.Public.Controllers;

public class HomeController : BaseController
{
    private readonly IProjectService _projectService;

    public HomeController(IProjectService projectService, IOptions<AppSettings> appSettings) : base(appSettings)
    {
        _projectService = projectService;
    }

    public async Task<IActionResult> Index()
    {
        var featuredProjects = await _projectService.GetFeaturedProjectsAsync();
        var otherProjects = await _projectService.GetOtherProjectsAsync();

        ViewBag.FeaturedProjects = featuredProjects;
        ViewBag.OtherProjects = otherProjects;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
