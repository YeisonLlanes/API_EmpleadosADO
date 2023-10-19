using System.Text.Json.Serialization;

namespace Api_EmpleadosADO.Models
{
    public class Departamento
    {

        public int IdDepartamento { get; set; }

        public string? Descripcion { get; set; }

        [JsonIgnore]
        public virtual ICollection<Cargo> Cargos { get; set; } = new List<Cargo>();
    }
}
