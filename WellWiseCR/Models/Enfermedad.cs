using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WellWiseCR.Models
{
    public class Enfermedad
    {
        [Required(ErrorMessage = "El campo ID de enfermedad es obligatorio.")]
        [Key]
        public int IdEnfermedad { get; set; }

        [Required(ErrorMessage = "El campo ID de especialidad es obligatorio.")]
        [ForeignKey("Especialidad")]
        public int IdEspecialidad { get; set; }

        [Required(ErrorMessage = "El campo nombre de enfermedad es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string NombreEnfermedad { get; set; }

        [Required(ErrorMessage = "El campo síntomas es obligatorio.")]
        [MaxLength(1000, ErrorMessage = "Puede ingresar un máximo de 1000 caracteres.")]
        public string Sintomas { get; set; }

        [Required(ErrorMessage = "El campo nivel de alerta es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string NivelAlerta { get; set; }

        [Required(ErrorMessage = "El campo recomendaciones es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 1000 caracteres.")]
        public string Recomendaciones { get; set; }

        [Required(ErrorMessage = "El campo estado es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Estado { get; set; }

        public Especialidad Especialidad { get; set; }

        public ICollection<Detalle> Detalle { get; set;}
    }
}
