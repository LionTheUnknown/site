using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    [Table("atlyginimas")]

    public class Salary
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("mokestis")]
        public float? cost { get; set; }

        [Column("bonusas")]
        public float? bonus { get; set; }

        [Column("fk_administratorius")]
        public int FkAdministratorius { get; set; }

        [Column("fk_administratorius1")]
        public int FkAdministratorius1 { get; set; }
    }
}