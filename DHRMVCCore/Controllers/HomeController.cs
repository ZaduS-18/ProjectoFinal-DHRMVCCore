using DHRMVCCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DHRMVCCore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult RedirectToListarUsuarios()
        {
            return RedirectToAction("ListarUsuarios", "Usuarios");
        }
        public IActionResult RecuperarPASS()
        {
            return RedirectToAction("RecuperarPass", "Usuarios");
        }
        public IActionResult RedirectToListarActums()
        {
            return RedirectToAction("ListarActas", "Actums");
        }

        public IActionResult RedirectToListarFechaCierre()
        {
            return RedirectToAction("ListarFechaCierre", "FechaCierres");
        }

        public IActionResult RedirectToListarEspecialidad()
        {
            return RedirectToAction("ListarEspecialidad", "Especialidads");
        }
        public IActionResult RedirectToListarRazonSocial()
        {
            return RedirectToAction("ListarRazonSocial", "RazonSocials");
        }
        public IActionResult RedirectToListarObras()
        {
            return RedirectToAction("ListarObras", "Obras");
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuarios");
        }

        public IActionResult Index()
        {
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
}