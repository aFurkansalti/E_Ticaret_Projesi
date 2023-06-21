using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using KategoriAPI.Models;

namespace KategoriAPI.Controllers
{
    public class KategorilerTempController : ApiController
    {
        private E_Ticaret_2023Entities db = new E_Ticaret_2023Entities();

        // GET: api/KategorilerTemp
        [ResponseType(typeof(Kategori))]
        public List<Kategori> GetKategoriler()
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



        // GET: api/KategorilerTemp/5
        [ResponseType(typeof(Kategori))]
        public IHttpActionResult GetKategoriler(int id)
        {
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            if (kategoriler == null)
            {
                return NotFound();
            }

            Kategori kategori = new Kategori()
            {
                KategoriID = kategoriler.KategoriID,
                KategoriAdi = kategoriler.KategoriAdi
            };

            return Ok(kategori);
        }

        // PUT: api/KategorilerTemp/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKategoriler(Kategoriler kategoriler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != kategoriler.KategoriID)
            //{
            //    return BadRequest();
            //}

            db.Entry(kategoriler).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KategorilerExists(kategoriler.KategoriID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/KategorilerTemp
        [ResponseType(typeof(Kategori))]
        public IHttpActionResult PostKategoriler(Kategoriler kategoriler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Kategoriler.Add(kategoriler);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = kategoriler.KategoriID }, kategoriler);
        }

        // DELETE: api/KategorilerTemp/5
        [ResponseType(typeof(Kategori))]
        public IHttpActionResult DeleteKategoriler(int id)
        {
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            if (kategoriler == null)
            {
                return NotFound();
            }

            db.Kategoriler.Remove(kategoriler);
            db.SaveChanges();

            return Ok(kategoriler);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KategorilerExists(int id)
        {
            return db.Kategoriler.Count(e => e.KategoriID == id) > 0;
        }
    }
}