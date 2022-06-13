using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb5.Models.Model
{
    [Table("Menu")]
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }
        public string Home { get; set; }
        [DisplayName("Read Product")]
        public string AboutUs { get; set; }
        [DisplayName("Products")]
        public string Services { get; set; }
        public string Blog { get; set; }
        public string Contact { get; set; }
        public int LanguageId { get; set; }
        [DisplayName("Dil")]
        public Language Language { get; set; }
    }
}