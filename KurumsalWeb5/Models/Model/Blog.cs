using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb5.Models.Model
{
    [Table("Blog")]
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        public string Baslik { get; set; }
        public string Baslik_en { get; set; }
        public string Icerik { get; set; }
        public string ResimURL { get; set; }
        public int LanguageId { get; set; }
        [DisplayName("Dil")]
        public Language language { get; set; }
        public int KategoriId { get; set; }
        [DisplayName("Kategori")]
        public Kategori kategori { get; set; }
        public ICollection<Yorum> Yorum { get; set; }
    }
}