using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibaryWithBorrowAspMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticRoles.ADMIN)]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
