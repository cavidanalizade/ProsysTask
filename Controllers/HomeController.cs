using System.Diagnostics;
using ImtahanProqrami.Data;
using ImtahanProqrami.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImtahanProqrami.Controllers
{
    public class HomeController : Controller
    {
        private readonly ImtahanContext db;

        public HomeController(ImtahanContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.DersSayi = await db.Dersler.CountAsync();
            ViewBag.SagirdSayi = await db.Sagirdler.CountAsync();
            ViewBag.ImtahanSayi = await db.Imtahanlar.CountAsync();
            ViewBag.QiymetsizSayi = await db.Imtahanlar.CountAsync(i => i.Qiymet == null);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var model = new ErrorViewModel();
            model.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            return View(model);
        }
    }
}
