using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellWiseCR.Models
{
    public class Especialista
    {
        [Required(ErrorMessage = "El campo ID de especialista es obligatorio.")]
        [Key]
        public int IdEspecialista { get; set; }

        [Required(ErrorMessage = "El campo especialidad es obligatorio.")]
        [ForeignKey("Especialidad")]
        public int IdEspecialidad { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo correo electrónico es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        [EmailAddress(ErrorMessage = "Dirección de correo electrónico inválida.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo nombre completo es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string NombreCompleto { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo provincia es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Provincia { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo cantón es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Canton { get; set; }

        [Required(ErrorMessage = "El campo estado es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Estado { get; set; }

        public Especialidad Especialidad { get; set; }
    }
}
