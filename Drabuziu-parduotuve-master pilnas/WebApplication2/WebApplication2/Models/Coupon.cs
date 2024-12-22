using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    [Table("nuolaidoskodas")] // Table name explicitly defined

    public class Coupon
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("sukurimodata")]
        [DataType(DataType.Date)]
        public DateTime Sukurimo_data { get; set; }

        [Column("veikimopradziosdata")]
        public Nullable<DateTime> Veikimo_pradzios_data { get; set; }

        [Column("panaudojimuskaičius")]
        public Nullable<int> Panaudojimu_sk { get; set; }

        [Column("kodas")]
        public string Kodas { get; set; }

        [Column("verte")]
        [Required]
        public double Verte { get; set; }

        [Column("aprasymas")]
		public string Aprasymas { get; set; }

        [Column("pavadinimas")]
        [Required]
        public string Pavadinimas { get; set; }

        [Column("galiojimopabaigosdata")]
        public Nullable<DateTime> Galiojimo_pabaigos_data { get; set; }

        [Column("yraribotas")]
        public bool Yra_ribotas { get; set; }

        public virtual ICollection<CouponProduct> CouponProducts { get; set; }

        [NotMapped]
		public List<int> SelectedProductIds { get; set; }
		
		[NotMapped]
		public Dictionary<int, int?> ProductMinQuantities { get; set; }
    }
}
