using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SwaggerUI.Center.Controllers;

/// <summary>
/// 首頁
/// </summary>
[Authorize]
public class HomeController : Controller
{
    /// <summary>
    /// index
    /// </summary>
    /// <returns></returns>
    public Task<IActionResult> Index()
    {
        return Task.FromResult<IActionResult>(new RedirectResult("~/swagger"));
    }
}