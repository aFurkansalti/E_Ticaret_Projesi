using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using E_Ticaret_2023.Models;
using Newtonsoft.Json;

namespace E_Ticaret_2023.Controllers
{
    [Authorize(Roles = "admin")]
    public class KategorilerController : Controller
    {
        private E_Ticaret_2023Entities db = new E_Ticaret_2023Entities();
        HttpClient client = new HttpClient();

        // GET: Kategoriler
        public ActionResult Index()
        {
            List<Kategoriler> liste = new List<Kategoriler>();
            client.BaseAddress = new Uri("https://localhost:44351/api/");
            var response = client.GetAsync("KategorilerTemp");  //asenkron metod nedir, asenkron nedir? araştır.
            response.Wait();

            if (response.Result.IsSuccessStatusCode)
            {
                var data = response.Result.Content.ReadAsStringAsync();
                data.Wait();
                liste = JsonConvert.DeserializeObject<List<Kategoriler>>(data.Result);
            }

            return View(liste);

            //return View(db.Kategoriler.ToList());
        }

        // GET: Kategoriler/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriler kategoriler = KategoriBul(id.Value);
            if (kategoriler == null)
            {
                return HttpNotFound();
            }
            return View(kategoriler);
        }

        private Kategoriler KategoriBul(int id)
        {
            Kategoriler kategori = null;
            // Kategoriler kategori = db.Kategoriler.Find(id);

            client.BaseAddress = new Uri("https://localhost:44351/api/");
            var response = client.GetAsync("KategorilerTemp/" + id.ToString());  //asenkron metod nedir, asenkron nedir? araştır.
            response.Wait();

            if (response.Result.IsSuccessStatusCode)
            {
                var data = response.Result.Content.ReadAsStringAsync();
                data.Wait();
                kategori = JsonConvert.DeserializeObject<Kategoriler>(data.Result);
            }

            return kategori;
        }

        // GET: Kategoriler/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kategoriler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KategoriID,KategoriAdi")] Kategoriler kategoriler)
        {
            if (ModelState.IsValid)
            {
                //db.Kategoriler.Add(kategoriler);
                //db.SaveChanges();
                //return RedirectToAction("Index");

                client.BaseAddress = new Uri("https://localhost:44351/api/");

                var response = client.PostAsJsonAsync<Kategoriler>("KategorilerTemp", kategoriler);
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

            }

            return View(kategoriler);
        }

        // GET: Kategoriler/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriler kategoriler = KategoriBul(id.Value);
            if (kategoriler == null)
            {
                return HttpNotFound();
            }
            return View(kategoriler);
        }

        // POST: Kategoriler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KategoriID,KategoriAdi")] Kategoriler kategoriler)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(kategoriler).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");

                kategoriler.KategoriAdi = kategoriler.KategoriAdi.Trim();
                client.BaseAddress = new Uri("https://localhost:44351/api/");
                var response = client.PutAsJsonAsync<Kategoriler>("KategorilerTemp", kategoriler);
                response.Wait();  //Asenkron olan uygulamayaı tamamlıyor ve bekliyor.

                if (response.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(kategoriler);
        }

        // GET: Kategoriler/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            if (kategoriler == null)
            {
                return HttpNotFound();
            }
            return View(kategoriler);
        }

        // POST: Kategoriler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Kategoriler kategoriler = db.Kategoriler.Find(id);
            //db.Kategoriler.Remove(kategoriler);
            //db.SaveChanges();

            client.BaseAddress = new Uri("https://localhost:44351/api/");
            var response = client.DeleteAsync("KategorilerTemp/" + id.ToString());
            response.Wait();

            if (response.Result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
