using Microsoft.AspNetCore.Mvc;
using Laboratorium09App.Models;
using Laboratorium09App.Data;
using Laboratorium09App.Helpers;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Laboratorium09App.Controllers
{
    public class IOController : Controller
    {
        private readonly AppDbContext _context;

        public IOController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Logowanie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logowanie(string login, string haslo)
        {
            var hash = HashHelper.ObliczHash(haslo);
            var user = _context.Loginy.FirstOrDefault(u => u.Nazwa == login && u.HasloHash == hash);
            if (user != null)
            {
                HttpContext.Session.SetString("Zalogowany", "true");
                return RedirectToAction("Zalogowano");
            }

            ViewBag.Blad = "Błędny login lub hasło";
            return View();
        }

        public IActionResult Zalogowano()
        {
            if (HttpContext.Session.GetString("Zalogowany") != "true")
                return RedirectToAction("Logowanie");
            return View();
        }

        [HttpPost]
        public IActionResult Wyloguj()
        {
            HttpContext.Session.Remove("Zalogowany");
            return RedirectToAction("Logowanie");
        }
    }
}