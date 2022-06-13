using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb5.Models.Model
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [DisplayName("Kategori")]
        public string CategoryName { get; set; }
        [DisplayName("Kategori Foroğrafı")]
        public string PhotoURL { get; set; }
        public ICollection<Product> product { get; set; }
        public ICollection<PastOrder> pastOrder { get; set; }
    }
}