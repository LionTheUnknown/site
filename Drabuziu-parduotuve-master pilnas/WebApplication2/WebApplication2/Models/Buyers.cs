using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    [Table("pirkejas")]
    public class Buyers
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("gimimodata")]
        public DateTime Birthday { get; set; }

        [Column("vieta")]
        [StringLength(255)]
        public string Place { get; set; }

        [Column("parduotuveskreditas")]
        public double Credit { get; set; }

        [Column("lytis")]
        public int Gender { get; set; }

        [Column("fk_nuolaidoskodas")]
        public int Code { get; set; }
    }
}