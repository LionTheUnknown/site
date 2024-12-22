using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    [Table("vartotojas")]
    public class Users
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("vardas")]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Column("pavarde")]
        [StringLength(255)]
        public string LastName { get; set; }

        [Column("slaptazodis")]
        [StringLength(255)]
        public string Password { get; set; }

        [Column("telefononumeris")]
        [StringLength(255)]
        public string PhoneNumber { get; set; }

        [Column("fk_nuolaidoskodas")]
        public int Code { get; set; }
    }
}