using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace WellWiseCR.Models
{
    public class Especialidad
    {
        [Required]
        [Key]
        public int IdEspecialidad { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string NombreEspecialidad { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Puede ingresar un máximo de 1000 caracteres.")]
        public string Descripcion { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Estado { get; set; }
    }
}
