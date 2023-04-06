using System.ComponentModel.DataAnnotations;

namespace WellWiseCR.Models
{
    public class Usuario
    {
        [Required]
        [Key]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string NombreUsuario { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        [MinLength(8, ErrorMessage = "Debe ingresar un mínimo de 8 caracteres.")]
        public string Password { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        [Compare("Password")]
        public string ConfirmacionPassword { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string NombreCompleto { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Email { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Rol { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Estado { get; set; }
    }
}
