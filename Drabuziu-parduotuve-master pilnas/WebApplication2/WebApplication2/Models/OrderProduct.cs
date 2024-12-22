using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    [Table("uzsakymasproduktas")] // Table name explicitly defined

    public class OrderProduct
    {
        [Column("fk_uzsakymas")] // Maps to the database column
        public int OrderId { get; set; }

        [Column("fk_produktas")] // Maps to the database column
        public int ProductId { get; set; }

        [Column("kaina")] // Maps to the database column
        public float cost { get; set; }

        public Product Product { get; set; }
        public Order Order{ get; set; }
    }
}