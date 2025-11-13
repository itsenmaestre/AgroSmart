using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_ENTITY.ENTIDADES_DTOS
{
    public class GastoTareaDTO
    {
        public int IdTarea { get; set; }
        public string TipoActividad { get; set; }
        public string NombreCultivo { get; set; }
        public DateTime FechaProgramada { get; set; }
        public string Estado { get; set; }
        public decimal TotalInsumos { get; set; }
        public decimal TotalManoObra { get; set; }
        public decimal CostoTransporte { get; set; }
        public decimal TotalGasto { get; set; }
        public List<GastoInsumoDTO> DetallesInsumos { get; set; } = new List<GastoInsumoDTO>();
        public List<GastoEmpleadoDTO> DetallesEmpleados { get; set; } = new List<GastoEmpleadoDTO>();
    }
}
