using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;

namespace WellWiseCR.Models
{
    public class Diagnostico
    {
        [Required]
        [Key]
        public int IdDiagnostico { get; set; }

        [Required(ErrorMessage = "El campo nombre de usuario es obligatorio.")]
        [ForeignKey("Usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El campo fecha y hora es obligatorio.")]
        [DataType(DataType.DateTime)]
        public DateTime FechaHora { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo peso es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        [Range(0, int.MaxValue, ErrorMessage = "El peso debe ser un valor numérico.")]
        public string Peso { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo estatura es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        [Range(0, int.MaxValue, ErrorMessage = "La estatura debe ser un valor numérico.")]
        public string Estatura { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo actividad física es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string ActividadFisica { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo condición cardíaca es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string CondicionCardiaca { get; set; }

        [Required(ErrorMessage = "El campo estado es obligatorio.")]
        [MaxLength(150, ErrorMessage = "Puede ingresar un máximo de 150 caracteres.")]
        public string Estado { get; set; }

        public Usuario Usuario { get; set; }

        public ICollection<Detalle> Detalle { get; set; }
    }
}
