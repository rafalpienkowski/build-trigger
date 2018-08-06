using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BuildTrigger.Web.Models;
using BuildTrigger;
using Microsoft.Extensions.Logging;

namespace BuildTrigger.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBuildTrigger _bulidTrigger;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IBuildTrigger bulidTrigger, ILogger<HomeController> logger)
        {
            _logger = logger;
            _bulidTrigger = bulidTrigger;
        }

        public async Task<IActionResult> Index()
        {
            var buildModels = new List<BuildViewModel>();
            try
            {
                _logger.LogTrace("Downloading build definitions");
                var builds = await _bulidTrigger.GetBuildsAsync();
                buildModels = builds
                    .Select(b => new BuildViewModel{ Id = b.Id, Name = b.Name })
                    .OrderBy(b => b.Name)
                    .ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Can't get build definitions");
            }
            return View(buildModels);
        }

        [HttpGet]
        public async Task<IActionResult> TriggerBuild([FromQuery]int buildId)
        {
            try
            {
                _logger.LogTrace($"Triggering build: {buildId}");
                var response = await _bulidTrigger.TriggerAsync(buildId);
                return Json($"{response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Can't trigger a build");
                return Json("Something went wrong");
            }
        }

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
