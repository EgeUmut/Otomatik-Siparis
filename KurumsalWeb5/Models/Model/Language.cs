using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb5.Models.Model
{
    [Table("Language")]
    public class Language
    {
        [Key]
        public int LanguageId { get; set; }
        [DisplayName("Dil İsmi")]
        public string Baslik { get; set; }
        [DisplayName("Dil Kodu")]
        public string Language_code { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Menu> Menus { get; set; }
    }
}