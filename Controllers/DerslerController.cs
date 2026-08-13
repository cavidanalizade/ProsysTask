using ImtahanProqrami.Data;
using ImtahanProqrami.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImtahanProqrami.Controllers
{
    public class DerslerController : Controller
    {
        private readonly ImtahanContext db;

        public DerslerController(ImtahanContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index(short? sinif)
        {
            var sorgu = db.Dersler.AsNoTracking().AsQueryable();

            if (sinif != null)
            {
                sorgu = sorgu.Where(d => d.Sinif == sinif.Value);
            }

            ViewBag.Sinif = sinif;

            var siyahi = await sorgu.OrderBy(d => d.Sinif).ThenBy(d => d.DersAdi).ToListAsync();
            return View(siyahi);
        }

        public IActionResult Create()
        {
            //formada sinif bos qalmasin deye
            return View(new Ders { Sinif = 9 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ders ders)
        {
            if (await db.Dersler.AnyAsync(d => d.DersKodu == ders.DersKodu))
            {
                ModelState.AddModelError("DersKodu", "Bu kod artiq istifade olunub.");
            }

            if (!ModelState.IsValid)
                return View(ders);

            db.Dersler.Add(ders);
            await db.SaveChangesAsync();

            TempData["Mesaj"] = ders.DersAdi + " elave olundu.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var ders = await db.Dersler.FindAsync(id);

            if (ders == null)
                return NotFound();

            return View(ders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Ders ders)
        {
            //kod primary key-dir, deyisdirilmesine icaze vermirik
            if (id != ders.DersKodu)
                return NotFound();

            if (!ModelState.IsValid)
                return View(ders);

            var kohne = await db.Dersler.FindAsync(id);

            if (kohne == null)
                return NotFound();

            kohne.DersAdi = ders.DersAdi;
            kohne.Sinif = ders.Sinif;
            kohne.MuellimAdi = ders.MuellimAdi;
            kohne.MuellimSoyadi = ders.MuellimSoyadi;

            await db.SaveChangesAsync();

            TempData["Mesaj"] = "Melumat yenilendi.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var ders = await db.Dersler.AsNoTracking().FirstOrDefaultAsync(d => d.DersKodu == id);

            if (ders == null)
                return NotFound();

            //sehifede xeberdarliq gostermek ucun
            ViewBag.ImtahanSayi = await db.Imtahanlar.CountAsync(i => i.DersKodu == id);

            return View(ders);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ders = await db.Dersler.FindAsync(id);

            if (ders == null)
                return NotFound();

            //neticeler varsa dersi silmek olmaz, yoxsa imtahanlar havada qalir
            if (await db.Imtahanlar.AnyAsync(i => i.DersKodu == id))
            {
                TempData["Xeta"] = "Bu dersden imtahan neticeleri var, silinmedi.";
                return RedirectToAction("Index");
            }

            db.Dersler.Remove(ders);
            await db.SaveChangesAsync();

            TempData["Mesaj"] = "Ders silindi.";
            return RedirectToAction("Index");
        }
    }
}
