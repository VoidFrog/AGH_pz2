using Microsoft.AspNetCore.Mvc;
using Laboratorium09App.Models;
using Laboratorium09App.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Laboratorium09App.Controllers
{
    public class DaneController : Controller
    {
        private readonly AppDbContext _context;

        public DaneController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Zalogowany") != "true")
                return RedirectToAction("Logowanie", "IO");

            var dane = _context.Dane.ToList();
            return View(dane);
        }

        [HttpPost]
        public IActionResult Dodaj(string tresc)
        {
            if (HttpContext.Session.GetString("Zalogowany") != "true")
                return RedirectToAction("Logowanie", "IO");

            if (!string.IsNullOrWhiteSpace(tresc))
            {
                _context.Dane.Add(new Dane { Tresc = tresc });
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}