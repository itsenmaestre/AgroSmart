using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_ENTITY.ENTIDADES_DTOS
{
    public class GastoEmpleadoDTO
    {
        public int IdAsignacion { get; set; }
        public int IdTarea { get; set; }
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public decimal JornadasTrabajadas { get; set; }
        public decimal HorasTrabajadas { get; set; }
        public decimal? PagoAcordado { get; set; }
        public decimal MontoCalculado { get; set; }
    }
}
