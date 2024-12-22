using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    [Table("mokestis")] // Table name explicitly defined

    public class Payment
    {
        [Column("id")]
        public int id { get; set; }
        [Column("mokestis")]
        public float cost { get; set; }
        [Column("fk_mokejimotipas")] 
        public int FkMokejimoTipas { get; set; } 
        [Column("fk_pirkejas")] 
        public int FkPirkejas { get; set; } 
        [Column("fk_uzsakymas")] 
        public int FkUzsakymas { get; set; } 
    }
}