using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eLibrairie.Core.Data.Models
{
    [Table("Compte")]
    public class Compte
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}
