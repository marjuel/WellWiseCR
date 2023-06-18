using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace WellWiseCR.Models
{
    public class Especialidad
    {
        [Required]
        [Key]
        public int IdEspecialidad { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo nombre de especialidad es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string NombreEspecialidad { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo descripción es obligatorio.")]
        [MaxLength(1000, ErrorMessage = "Puede ingresar un máximo de 1000 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo estado es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Estado { get; set; }
    }
}
