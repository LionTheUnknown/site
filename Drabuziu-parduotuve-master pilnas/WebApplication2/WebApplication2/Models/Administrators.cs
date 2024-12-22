using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    [Table("administratorius")]
    public class Administrator
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("arvadovas")]
        public Boolean Chief { get; set; }

        [Column("atlyginimas")]
        public double Salary { get; set; }

        [Column("kortelesnumeris")]
        [StringLength(255)]
        public string Card { get; set; }
    }
}