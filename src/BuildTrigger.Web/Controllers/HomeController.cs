using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BuildTrigger.Web.Models;
using BuildTrigger;

namespace BuildTrigger.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBuildTrigger _bulidTrigger;

        public HomeController(IBuildTrigger bulidTrigger)
        {
            _bulidTrigger = bulidTrigger;
        }

        public async Task<IActionResult> Index()
        {
            var builds = await _bulidTrigger.GetBuildsAsync();
            return View(builds.OrderBy(b => b.Name).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> TriggerBuild([FromQuery]int buildId)
        {
            try
            {
                var response = await _bulidTrigger.TriggerAsync(buildId);
                return Json($"{response.StatusCode}");
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
