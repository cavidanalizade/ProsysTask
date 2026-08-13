using ImtahanProqrami.Data;
using ImtahanProqrami.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImtahanProqrami.Controllers
{
    public class SagirdlerController : Controller
    {
        private readonly ImtahanContext db;

        public SagirdlerController(ImtahanContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index(short? sinif, string axtaris)
        {
            var sorgu = db.Sagirdler.AsNoTracking().AsQueryable();

            if (sinif != null)
                sorgu = sorgu.Where(s => s.Sinif == sinif.Value);

            if (!string.IsNullOrWhiteSpace(axtaris))
            {
                axtaris = axtaris.Trim();
                sorgu = sorgu.Where(s => s.Adi.Contains(axtaris) || s.Soyadi.Contains(axtaris));
            }

            ViewBag.Sinif = sinif;
            ViewBag.Axtaris = axtaris;

            return View(await sorgu.OrderBy(s => s.Soyadi).ThenBy(s => s.Adi).ToListAsync());
        }

        public IActionResult Create()
        {
            return View(new Sagird { Sinif = 9 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sagird sagird)
        {
            if (await db.Sagirdler.AnyAsync(s => s.Nomre == sagird.Nomre))
            {
                ModelState.AddModelError("Nomre", "Bu nomre artiq baskasina verilib.");
            }

            if (!ModelState.IsValid)
                return View(sagird);

            db.Sagirdler.Add(sagird);
            await db.SaveChangesAsync();

            TempData["Mesaj"] = sagird.TamAd + " elave olundu.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sagird = await db.Sagirdler.FindAsync(id);

            if (sagird == null)
                return NotFound();

            return View(sagird);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sagird sagird)
        {
            if (id != sagird.Nomre)
                return NotFound();

            if (!ModelState.IsValid)
                return View(sagird);

            var kohne = await db.Sagirdler.FindAsync(id);

            if (kohne == null)
                return NotFound();

            kohne.Adi = sagird.Adi;
            kohne.Soyadi = sagird.Soyadi;
            kohne.Sinif = sagird.Sinif;

            await db.SaveChangesAsync();

            TempData["Mesaj"] = "Melumat yenilendi.";
            return RedirectToAction("Index");
        }

        //sagirdin butun imtahanlarini bir sehifede gostermek ucun
        public async Task<IActionResult> Detay(int id)
        {
            var sagird = await db.Sagirdler
                                 .AsNoTracking()
                                 .Include(s => s.Imtahanlar)
                                    .ThenInclude(i => i.Ders)
                                 .FirstOrDefaultAsync(s => s.Nomre == id);

            if (sagird == null)
                return NotFound();

            return View(sagird);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var sagird = await db.Sagirdler.AsNoTracking().FirstOrDefaultAsync(s => s.Nomre == id);

            if (sagird == null)
                return NotFound();

            ViewBag.ImtahanSayi = await db.Imtahanlar.CountAsync(i => i.SagirdNomresi == id);

            return View(sagird);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sagird = await db.Sagirdler.FindAsync(id);

            if (sagird == null)
                return NotFound();

            if (await db.Imtahanlar.AnyAsync(i => i.SagirdNomresi == id))
            {
                TempData["Xeta"] = "Sagirdin imtahan neticeleri var, silinmedi.";
                return RedirectToAction("Index");
            }

            db.Sagirdler.Remove(sagird);
            await db.SaveChangesAsync();

            TempData["Mesaj"] = "Sagird silindi.";
            return RedirectToAction("Index");
        }
    }
}
