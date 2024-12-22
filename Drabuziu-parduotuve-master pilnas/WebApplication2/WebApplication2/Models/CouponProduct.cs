using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("nuolaidoskodas_produktas")]
    public class CouponProduct
    {
        [Key, Column("fk_nuolaidoskodas", Order = 0)]
        public int CouponId { get; set; }

        [Key, Column("fk_produktas", Order = 1)]
        public int ProductId { get; set; }

        [Column("minkiekis")]
        public int? MinQuantity { get; set; }

        public virtual Coupon Coupon { get; set; }
        public virtual Product Product { get; set; }
    }
}