using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KurumsalWeb5.Models.Model
{
    public class PastOrder
    {
        [Key]
        public int PastOrderId { get; set; }
        public Category category { get; set; }
        public int CategoryId { get; set; }
        [DisplayName("Ürün Adı")]
        public string ProductName { get; set; }
        [DisplayName("Ürün Sayısı")]
        public int quantity { get; set; }
        [DisplayName("Toplam Ücret (₺)")]
        public decimal PriceSum { get; set; }
        [DisplayName("Sipariş Zamanı")]
        public DateTime Time { get; set; }
    }
}