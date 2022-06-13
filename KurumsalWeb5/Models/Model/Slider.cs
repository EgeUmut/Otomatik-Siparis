using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb5.Models.Model
{
    [Table("Slider")]
    public class Slider
    {
        
        [Key]
        public int SliderId { get; set; }
        [DisplayName("Slider Başlık"),StringLength(30,ErrorMessage = "max 30 karakter")]
        public string Baslik { get; set; }
        [DisplayName("Slider Açıklama"), StringLength(200, ErrorMessage = "max 200 karakter")]
        public string Aciklama { get; set; }
        [DisplayName("Slider Resim"), StringLength(250, ErrorMessage = "max 250 karakter")]
        public string ResimURL { get; set; }
    }
}