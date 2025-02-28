using System.ComponentModel.DataAnnotations;

namespace Aspire_POS.Models
{
    public class DBSourceModel
    {
        [Key]
        public int Id { get; set; } // Identificador único

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // Nombre de la fuente (Ej: SQL Server, WooCommerce, etc.)

        [StringLength(255)]
        public string Description { get; set; } // Descripción opcional

        public bool IsAvailable { get; set; } = true; // Indica si la fuente está disponible (1 = Sí, 0 = No)

        // Relación con los usuarios (muchos a muchos)
        public ICollection<UserDBSourceModel> UserDBSources { get; set; }
    }
}
