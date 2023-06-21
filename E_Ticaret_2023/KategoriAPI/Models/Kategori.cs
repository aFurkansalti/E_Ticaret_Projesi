using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KategoriAPI.Models
{
    public class Kategori
    {
        public int KategoriID { get; set; }

        public string KategoriAdi { get; set; }

    }
}