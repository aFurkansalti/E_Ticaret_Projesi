using E_Ticaret_2023.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace E_Ticaret_2023.Controllers
{
    [Authorize]  //Cookie olmadığı için verdik
    public class SiparisController : Controller
    {
        E_Ticaret_2023Entities db = new E_Ticaret_2023Entities();
        // GET: Siparis
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var sepet = db.Siparis.Where(x => x.KullaniciID == userId);
            return View(db.Siparis.ToList());
        }

        public ActionResult Details(int id)
        {
            var siparisDetay = db.SiparisDetay.Where(x => x.SiparisID == id);
            return View(siparisDetay.ToList());
        }

        public ActionResult SiparisTamamla()
        {
            //    ClientID: Bankadan alınan mağaza kodu
            //    Amount:Sepetteki ürünlerin toplam tutar
            //    Oid:SiparişID
            //    OnayUrl:Ödeme başarılı olduğunda gelen verilerin gösterileceği url
            //    HataUrl:Ödeme sırasında hata olduysa gelen hatanın gösterileceği url
            //    RDN:Hash karşılaştırılıması için kullanılan bilgi
            //        StoreKEy:Güvenlik anahtarı.Bankanın sanal pos sayfasından alınır.
            //        TransactionType:"Auth"
            //        Instalment:""
            //        HashStr:HashSet oluşturulurken bankanın istediği bilgiler birleştirilir.
            //        Hash:Farklı değerler oluşturulup birleştirilir.

            string userID = User.Identity.GetUserId();

            List<Sepet> sepetUrunleri = db.Sepet.Where(x => x.KullaniciID == userID).ToList();

            string ClientId = "1003001";//Bankanın verdiği magaza kodu
            string ToplamTutar = sepetUrunleri.Sum(x => x.ToplamTutar).ToString();

            string sipId = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);

            string onayURL = "https://localhost:44354/Siparis/Tamamlandi";

            string hataURL = "https://localhost:44354/Siparis/Hatali";

            string RDN = "asdf";
            string StoreKey = "123456";

            string TransActionType = "Auth";
            string Instalment = "";

            string HashStr = ClientId + sipId + ToplamTutar + onayURL + hataURL + TransActionType + Instalment + RDN + StoreKey;//Bankanın istediği bilgiler

            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();

            byte[] HashBytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(HashStr);
            byte[] InputBytes = sha.ComputeHash(HashBytes);
            string Hash = Convert.ToBase64String(InputBytes);

            ViewBag.ClientId = ClientId;
            ViewBag.Oid = sipId;
            ViewBag.okUrl = onayURL;
            ViewBag.failUrl = hataURL;
            ViewBag.TransActionType = TransActionType;
            ViewBag.RDN = RDN;
            ViewBag.Hash = Hash;
            ViewBag.Amount = ToplamTutar;
            ViewBag.StoreType = "3d_pay_hosting"; // Ödeme modelimiz
            ViewBag.Description = "";
            ViewBag.XID = "";
            ViewBag.Lang = "tr";
            ViewBag.EMail = "cenelif@gmail.com";
            ViewBag.UserID = "ElifCengiz"; // bu id yi bankanın sanala pos ekranında biz oluşturuyoruz.
            ViewBag.PostURL = "https://localhost:44354/Siparis/Tamamlandi";
                // "https://entegrasyon.asseco-see.com.tr/fim/est3Dgate";

            return View();
        }

        public ActionResult Tamamlandi()
        {
            string ad = Request.Form.Get("Ad");
            string soyad = Request.Form.Get("Soyad");
            string adres = Request.Form.Get("Adres");
            string telefon = Request.Form.Get("Telefon");
            string tcno = Request.Form.Get("TCKimlikNo");
            string userID = User.Identity.GetUserId();

            Siparis siparis = new Siparis() { Ad = ad, Soyad = soyad, Adres = adres, Telefon = telefon, TCKimlikNo = tcno, Tarih = DateTime.Now, KullaniciID = userID };

            List<Sepet> sepettekiurunler = db.Sepet.Where(x => x.KullaniciID == userID).ToList();

            foreach (var item in sepettekiurunler)
            {
                SiparisDetay sd = new SiparisDetay()
                {
                    UrunID = item.UrunID,
                    Adet = item.Adet,
                    ToplamTutar = item.ToplamTutar
                };
                siparis.SiparisDetay.Add(sd);

                db.Sepet.Remove(item);
            }

            db.Siparis.Add(siparis);
            db.SaveChanges();

            return View();
        }

        public ActionResult Hatali()
        {
            ViewBag.hata = Request.Form;
            return View();
        }
    }
}