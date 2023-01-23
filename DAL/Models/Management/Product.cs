using System.ComponentModel.DataAnnotations;

namespace DAL.Models{
    public class Product{
        [Key]
        public long Id {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
    }
}