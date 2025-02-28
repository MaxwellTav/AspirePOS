using System.ComponentModel.DataAnnotations;

namespace Aspire_POS.Models
{
    public class UserDBSourceModel
    {
        [Key]
        public int Id { get; set; } // Identificador único

        public string UserId { get; set; } // ID del usuario
        public int DBSourceId { get; set; } // ID de la fuente de datos

        public bool IsAvailableForUser { get; set; } // Indica si la fuente está disponible para el usuario

        // Relaciones
        public DBSourceModel DBSource { get; set; }
    }
}
