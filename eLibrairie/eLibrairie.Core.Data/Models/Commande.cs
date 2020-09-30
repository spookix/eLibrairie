using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eLibrairie.Core.Data.Models
{
    [Table("Commande")]
    public class Commande
    {
        [Key]
        public int Id { get; set; }
        public string CompteMail { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}
