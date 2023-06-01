using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductWebApi.Models
{
    [Table("product")]
    public class Product
    {
        [Key]
        [Column("product")]
        public int ProductId { get; set; }
        [Column("product_name")]
        public string ProductName { get; set; }
        [Column("product_code")]
        public string ProductCode { get; set; }
        [Column("product_price")]
        public decimal Productprice { get; set; }
    }
}
