using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb5.Models.Model
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [DisplayName("Barkod")]
        public string BarcodeNumber { get; set; }
        [DisplayName("Ürün Adı")]
        public string ProductName { get; set; }
        [DisplayName("Ürün Açıklaması")]
        public string  Description { get; set; }
        [DisplayName("Kategori")]
        public int CategoryId { get; set; }
        [DisplayName("Kategori")]
        public Category category { get; set; }
        [DisplayName("Fiyat (₺)")]
        public decimal UnitPrice { get; set; }
        [DisplayName("Mevcut Ürün sayısı")]
        public int quantity { get; set; }
        [DisplayName("İstenilen ürün sayısı")]
        public int Defaultquantity { get; set; }
        [DisplayName("Ürün limiti")]
        [Range(0,100, ErrorMessage = "Limit 0-100 arasında olmalıdır")]
        public int Limit { get; set; }

    }
}