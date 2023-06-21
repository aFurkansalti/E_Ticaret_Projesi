using KategoriAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web.Http;
using System.Web.Mvc;

namespace KategoriAPI.Controllers
{
    public class KategorilerController : ApiController
    {
        // Öncelikle get yazacağız, database'deki kategorileri almak için

        E_Ticaret_2023Entities db = new E_Ticaret_2023Entities();
        public List<Kategori> Get()
        {
            List<Kategoriler> liste = this.db.Kategoriler.ToList();  // Eğer listeyi döndürürsen hata alırsın, istersen dene
            List<Kategori> kategoriler = new List<Kategori>();

            foreach (var item in liste)
            {
                kategoriler.Add(new Kategori()
                {
                    KategoriID = item.KategoriID,
                    KategoriAdi = item.KategoriAdi
                });
            }

            //kategoriler = (from x in db.Kategoriler
            //               select new Kategori
            //               { KategoriID = x.KategoriID, KategoriAdi = x.KategoriAdi }).ToList();

            return kategoriler;
        }

        public Kategori Get(int id)
        {
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            Kategori kategori = new Kategori()
            {
                KategoriID = kategoriler.KategoriID,
                KategoriAdi = kategoriler.KategoriAdi
            };

            return kategori;
        }


    }
}
