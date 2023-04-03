using System;
using System.ComponentModel.DataAnnotations;

namespace WellWiseCR.Models
{
    public class Usuario
    {
        //Propiedades de la clase Usuario
        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string NombreUsuario { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        [MinLength(8, ErrorMessage = "Debe ingresar un mínimo de 8 caracteres.")]
        public string Password { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        [Compare("Password", ErrorMessage = "La contraseña no coincide.")]
        public string ConfirmacionPassword { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        [EmailAddress(ErrorMessage = "El formato de correo electrónico es incorrecto.")]
        public string Email { get; set; }

        
        [Required]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Rol { get; set; }

        
        [Required]
        public string Estado { get; set; }
    }//Fin de la clase Usuario
}//Fin del namespace
