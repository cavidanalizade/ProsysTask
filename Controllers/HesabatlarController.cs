using ImtahanProqrami.Data;
using ImtahanProqrami.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImtahanProqrami.Controllers
{
    //SQL faylinin sonundaki hesabat sorgularinin proqram variantidir
    public class HesabatlarController : Controller
    {
        private readonly ImtahanContext db;

        public HesabatlarController(ImtahanContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HesabatGorunusu();

            //qiymeti qoyulmayan imtahanlar orta bala tesir etmemelidir
            var dersUzre = await db.Dersler.AsNoTracking()
                .Select(d => new DersOrtaBal
                {
                    DersAdi = d.DersAdi,
                    ImtahanSayi = d.Imtahanlar.Count(i => i.Qiymet != null),
                    OrtaBal = d.Imtahanlar.Where(i => i.Qiymet != null).Average(i => (decimal?)i.Qiymet)
                })
                .ToListAsync();

            model.DersUzre = dersUzre.OrderByDescending(x => x.OrtaBal ?? 0).ToList();

            var sagirdUzre = await db.Sagirdler.AsNoTracking()
                .Select(s => new SagirdOrtaBal
                {
                    Nomre = s.Nomre,
                    Adi = s.Adi,
                    Soyadi = s.Soyadi,
                    Sinif = s.Sinif,
                    ImtahanSayi = s.Imtahanlar.Count(i => i.Qiymet != null),
                    OrtaBal = s.Imtahanlar.Where(i => i.Qiymet != null).Average(i => (decimal?)i.Qiymet)
                })
                .ToListAsync();

            model.SagirdUzre = sagirdUzre.OrderByDescending(x => x.OrtaBal ?? 0).ToList();

            model.Kesirler = await db.Imtahanlar.AsNoTracking()
                .Include(i => i.Ders)
                .Include(i => i.Sagird)
                .Where(i => i.Qiymet == 2)
                .OrderBy(i => i.Sagird!.Soyadi)
                .ToListAsync();

            model.Imtahansizlar = await db.Sagirdler.AsNoTracking()
                .Where(s => !s.Imtahanlar.Any())
                .OrderBy(s => s.Sinif).ThenBy(s => s.Soyadi)
                .ToListAsync();

            return View(model);
        }
    }
}
