using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    [Table("sandelisproduktas")] // Table name explicitly defined

    public class WarehouseProduct
    {
        [Column("fk_sandelis")] // Maps to the database column
        public int WarehouseId { get; set; }

        [Column("fk_produktas")] // Maps to the database column
        public int ProductId { get; set; }

        [Column("kiekis")] // Maps to the database column
        public int Count { get; set; }

        public Product Product { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}