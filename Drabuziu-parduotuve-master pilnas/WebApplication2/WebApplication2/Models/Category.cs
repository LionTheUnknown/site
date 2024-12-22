using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    [Table("kategorija")] // Table name explicitly defined

    public class Category
    {
        [Column("id")]
        public int id { get; set; }
        [Column("pavadinimas")]
        public string name { get; set; }
        [Column("aprasas")]
        public string description { get; set; }
        [Column("fk_tevinekategorija")]
        public int? parentId { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

        public Category Parent { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        /*public Product Products { get; set; }

        public Category[] getParents()
        {
            return new Category[0];
        }
        public Category[] getChildren()
        {
            return  new Category[0];
        }
        public Product[] getProducts()
        {
            return new Product[0];
        }*/
    }
}