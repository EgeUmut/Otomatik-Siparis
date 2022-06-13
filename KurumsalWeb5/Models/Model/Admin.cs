using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb5.Models.Model
{
    [Table("Admin")]
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        [Required,StringLength(50,ErrorMessage ="50 karakter maksimum")]
        public string Eposta { get; set; }
        [Required, StringLength(50, ErrorMessage = "50 karakter maksimum")]
        public string Sifre { get; set; }
        [Required, StringLength(50, ErrorMessage = "50 karakter maksimum")]
        public string Yetki { get; set; }
    }
}