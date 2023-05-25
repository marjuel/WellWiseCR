using System.ComponentModel.DataAnnotations.Schema;

namespace WellWiseCR.Models
{
    public class Detalle
    {
        public int IdDiagnostico { get; set; }
        public int IdEnfermedad { get; set; }
        public Diagnostico Diagnostico{ get; set; }
        public Enfermedad Enfermedad{ get; set; }
    }
}
