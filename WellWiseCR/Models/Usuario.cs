using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace WellWiseCR.Models
{
    public class Usuario
    {
        [Required (ErrorMessage = "El campo nombre de usuario es obligatorio.")]
        [Key]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El campo contraseña es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        [MinLength(8, ErrorMessage = "Debe ingresar un mínimo de 8 caracteres, incluyendo al menos una mayúscula, una minúscula y un número.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo confirmacion de contraseña es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        [Compare("Password", ErrorMessage = "La confirmación de contraseña no coincide.")]
        public string ConfirmacionPassword { get; set; }

        [Required(ErrorMessage = "El campo nombre completo es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El campo correo electrónico es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo rol es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Rol { get; set; }

        [Required(ErrorMessage = "El campo estado es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Estado { get; set; }
    }
}
