using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    [Table("sandelis")] // Table name explicitly defined

    public class Warehouse
    {
        [Column("id")]
        public int id { get; set; }
        [Column("pavadinimas")]
        public string name { get; set; }
        [Column("vieta")]
        public string place { get; set; }
        public virtual ICollection<WarehouseProduct> WarehouseProducts { get; set; }
    }
}