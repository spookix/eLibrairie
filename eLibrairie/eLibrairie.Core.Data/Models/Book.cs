using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eLibrairie.Core.Data.Models
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage ="Le nom est requis")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Le prix est requis")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "L'image est requise")]
        public string Image { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La catégorie est requise")]
        public int CategorieId { get; set; }

        public string Description { get; set; }
    }
}
