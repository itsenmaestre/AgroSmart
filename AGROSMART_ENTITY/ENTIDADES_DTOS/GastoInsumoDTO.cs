using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_ENTITY.ENTIDADES_DTOS
{
    public class GastoInsumoDTO
    {
        public int IdDetalle { get; set; }
        public int IdTarea { get; set; }
        public int IdInsumo { get; set; }
        public string NombreInsumo { get; set; }
        public string TipoInsumo { get; set; }
        public decimal CantidadUsada { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal CostoTotal { get; set; }
    }
}
