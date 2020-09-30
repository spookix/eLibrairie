using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eLibrairie.Core.Data.Models
{
    [Table("Categorie")]
    public class Categorie
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Le nom est requis")]
        public string Name { get; set; }

        public List<Book> TheBooks { get; set; }
    }
}
