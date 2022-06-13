using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb5.Models.Model
{
    [Table("Kimlik")]
    public class Kimlik
    {   
        [Key]
        public int KimlikId { get; set; }
        [Required, StringLength(100, ErrorMessage = "100 karakter maksimum")]
        [DisplayName("Site Başlık")]
        public string Title { get; set; }
        [Required, StringLength(200, ErrorMessage = "2000 karakter maksimum")]
        [DisplayName("Anahtar Kelimeler")]
        public string Keywords { get; set; }
        [Required, StringLength(300, ErrorMessage = "300 karakter maksimum")]
        [DisplayName("Site Açıklama")]
        public string Description { get; set; }
        
        [DisplayName("Site Logo")]
        public string LogoURL { get; set; }
        [DisplayName("Site Unvan")]
        public string Unvan { get; set; }
    }
}