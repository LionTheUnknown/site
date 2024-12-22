using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("produktas")] // Table name explicitly defined
    public class Product
    {
        [Column("id")]
        public int id { get; set; }
        [Column("pavadinimas")]
        public string name { get; set; }
        [Column("aprasas")]
        [StringLength(500)]
        public string description { get; set; }
        [Column("kaina")]
        [Required]
        [Range(0, 10000)]
        public double price { get; set; }


        [Column("mase")]
        [Range(0, int.MaxValue)]
        public double mass { get; set; }
        public virtual ICollection<WarehouseProduct> WarehouseProducts { get; set; }

        public int getAmount() {
            int amount = 0;
            foreach(WarehouseProduct entry in WarehouseProducts)
            {
                amount+=entry.Count;
            }
            return amount;
        }

        [Column("gamintojas")]
        public string manufacturer { get; set; }
        [Column("nuotraukos_url")]
        public string imageURL{ get; set; }
        [Column("fk_pardavėjas")]
        public int? seller { get; set; }


        //public Category[] Categories { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public Product()
        {
            //ProductCategories = new HashSet<ProductCategory>();
            //Categories=
        }
    }
}