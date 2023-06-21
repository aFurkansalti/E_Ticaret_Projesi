using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Ticaret_2023.Models;

namespace E_Ticaret_2023.Controllers
{
    [Authorize]
    public class SepetController : Controller
    {
        // GET: Sepet
        E_Ticaret_2023Entities db = new E_Ticaret_2023Entities();
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var sepet = db.Sepet.Where(x => x.KullaniciID == userId);
            return View(db.Sepet.ToList());
        }

        public ActionResult SepeteEkle(int urunID, int adet)
        {
            string userID = User.Identity.GetUserId();
            Urunler urun = db.Urunler.Find(urunID);

            Sepet sepettekiUrun = db.Sepet.FirstOrDefault(x => x.UrunID == urunID && x.KullaniciID == userID);

            if (sepettekiUrun == null)
            {
                Sepet sepet = new Sepet()
                {
                    KullaniciID = userID,
                    UrunID = urunID,
                    Adet = adet,
                    ToplamTutar = adet * urun.UrunFiyati
                };
                db.Sepet.Add(sepet);
            }
            else
            {
                sepettekiUrun.Adet += adet;
                sepettekiUrun.ToplamTutar = sepettekiUrun.Adet * urun.UrunFiyati;
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult SepetGuncelle(int id, int adet)
        {
            Sepet sepet = db.Sepet.Find(id);
            if (sepet == null)
            {
                return HttpNotFound();
            }

            Urunler urun = db.Urunler.Find(sepet.UrunID);

            sepet.Adet = adet;
            sepet.ToplamTutar = sepet.Adet * urun.UrunFiyati;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult SepetSil(int id)
        {
            Sepet sepet = db.Sepet.Find(id);

            if (sepet != null)
            {
                db.Sepet.Remove(sepet);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}