using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_ENTITY.ENTIDADES
{
    public class COSECHA
    {
        public int ID_COSECHA { get; set; }
        public int ID_CULTIVO { get; set; }
        public int ID_ADMIN_REGISTRO { get; set; }
        public DateTime FECHA_COSECHA { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
        public decimal CANTIDAD_OBTENIDA { get; set; } // NUMBER(14,2)
        public string UNIDAD_MEDIDA { get; set; }
        public string CALIDAD { get; set; }
        public string OBSERVACIONES { get; set; }
    }
}
