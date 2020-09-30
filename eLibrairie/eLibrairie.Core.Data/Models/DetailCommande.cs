using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eLibrairie.Core.Data.Models
{
    [Table("DetailCommande")]
    public class DetailCommande
    {
        [Key]
        public int Id { get; set; }
        public int CommandeId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal PrixTotal { get; set; }
    }
}
