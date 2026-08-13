using ImtahanProqrami.Data;
using ImtahanProqrami.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ImtahanProqrami.Controllers
{
    public class ImtahanlarController : Controller
    {
        private readonly ImtahanContext db;

        public ImtahanlarController(ImtahanContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index(string dersKodu)
        {
            //ders ve sagird adlarini gostermek ucun Include lazimdir
            var sorgu = db.Imtahanlar.AsNoTracking()
                                     .Include(i => i.Ders)
                                     .Include(i => i.Sagird)
                                     .AsQueryable();

            if (!string.IsNullOrEmpty(dersKodu))
                sorgu = sorgu.Where(i => i.DersKodu == dersKodu);

            ViewBag.Dersler = await DersleriGetir(dersKodu);

            return View(await sorgu.OrderByDescending(i => i.ImtahanTarixi).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            await SiyahilariHazirla(null);

            var imtahan = new Imtahan();
            imtahan.ImtahanTarixi = DateOnly.FromDateTime(DateTime.Today);

            return View(imtahan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Imtahan imtahan)
        {
            if (!await db.Dersler.AnyAsync(d => d.DersKodu == imtahan.DersKodu))
                ModelState.AddModelError("DersKodu", "Bele ders yoxdur.");

            if (!await db.Sagirdler.AnyAsync(s => s.Nomre == imtahan.SagirdNomresi))
                ModelState.AddModelError("SagirdNomresi", "Bele sagird yoxdur.");

            //acar 3 sutundan ibaret oldugu ucun tekrari ozumuz yoxlayiriq
            bool varmi = await db.Imtahanlar.AnyAsync(i => i.DersKodu == imtahan.DersKodu
                                                        && i.SagirdNomresi == imtahan.SagirdNomresi
                                                        && i.ImtahanTarixi == imtahan.ImtahanTarixi);
            if (varmi)
                ModelState.AddModelError("", "Bu sagird hemin gun bu dersden artiq imtahan verib.");

            if (!ModelState.IsValid)
            {
                await SiyahilariHazirla(imtahan);
                return View(imtahan);
            }

            db.Imtahanlar.Add(imtahan);
            await db.SaveChangesAsync();

            TempData["Mesaj"] = "Imtahan yazildi.";
            return RedirectToAction("Index");
        }

        //qiymeti sonradan qoymaq / duzeltmek ucun
        public async Task<IActionResult> Edit(string dersKodu, int sagirdNomresi, DateOnly tarix)
        {
            var imtahan = await Tap(dersKodu, sagirdNomresi, tarix);

            if (imtahan == null)
                return NotFound();

            return View(imtahan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string dersKodu, int sagirdNomresi, DateOnly tarix, short? qiymet)
        {
            //bos deyer qadagan deyil - sehv qoyulmus qiymeti geri goturmek lazim ola biler
            if (qiymet.HasValue && (qiymet < 2 || qiymet > 5))
            {
                ModelState.AddModelError("Qiymet", "Qiymet 2 ile 5 arasinda olmalidir.");

                var kohne = await Tap(dersKodu, sagirdNomresi, tarix);
                if (kohne == null)
                    return NotFound();

                return View(kohne);
            }

            var imtahan = await db.Imtahanlar.FirstOrDefaultAsync(i => i.DersKodu == dersKodu
                                                                    && i.SagirdNomresi == sagirdNomresi
                                                                    && i.ImtahanTarixi == tarix);
            if (imtahan == null)
                return NotFound();

            imtahan.Qiymet = qiymet;
            await db.SaveChangesAsync();

            if (qiymet.HasValue)
                TempData["Mesaj"] = "Qiymet yazildi.";
            else
                TempData["Mesaj"] = "Qiymet geri goturuldu.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string dersKodu, int sagirdNomresi, DateOnly tarix)
        {
            var imtahan = await Tap(dersKodu, sagirdNomresi, tarix);

            if (imtahan == null)
                return NotFound();

            return View(imtahan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string dersKodu, int sagirdNomresi, DateOnly tarix)
        {
            var imtahan = await db.Imtahanlar.FirstOrDefaultAsync(i => i.DersKodu == dersKodu
                                                                    && i.SagirdNomresi == sagirdNomresi
                                                                    && i.ImtahanTarixi == tarix);
            if (imtahan == null)
                return NotFound();

            db.Imtahanlar.Remove(imtahan);
            await db.SaveChangesAsync();

            TempData["Mesaj"] = "Netice silindi.";
            return RedirectToAction("Index");
        }

        //acar 3 sutundan ibaret oldugu ucun FindAsync yerine bunu yazdim
        private async Task<Imtahan?> Tap(string dersKodu, int sagirdNomresi, DateOnly tarix)
        {
            if (string.IsNullOrEmpty(dersKodu))
                return null;

            return await db.Imtahanlar.AsNoTracking()
                                      .Include(i => i.Ders)
                                      .Include(i => i.Sagird)
                                      .FirstOrDefaultAsync(i => i.DersKodu == dersKodu
                                                             && i.SagirdNomresi == sagirdNomresi
                                                             && i.ImtahanTarixi == tarix);
        }

        private async Task SiyahilariHazirla(Imtahan? secilmis)
        {
            ViewBag.Dersler = await DersleriGetir(secilmis?.DersKodu);

            var sagirdler = await db.Sagirdler.AsNoTracking()
                                              .OrderBy(s => s.Soyadi)
                                              .Select(s => new
                                              {
                                                  s.Nomre,
                                                  Ad = s.Nomre + " - " + s.Adi + " " + s.Soyadi
                                              })
                                              .ToListAsync();

            ViewBag.Sagirdler = new SelectList(sagirdler, "Nomre", "Ad", secilmis?.SagirdNomresi);
        }

        private async Task<SelectList> DersleriGetir(string? secilmis)
        {
            var dersler = await db.Dersler.AsNoTracking()
                                          .OrderBy(d => d.DersAdi)
                                          .Select(d => new { d.DersKodu, d.DersAdi })
                                          .ToListAsync();

            return new SelectList(dersler, "DersKodu", "DersAdi", secilmis);
        }
    }
}
