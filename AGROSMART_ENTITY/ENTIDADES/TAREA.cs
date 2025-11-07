using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_ENTITY.ENTIDADES
{
    public class TAREA
    {
        public int ID_TAREA { get; set; }
        public int ID_CULTIVO { get; set; }
        public int ID_ADMIN_CREADOR { get; set; }
        public string TIPO_ACTIVIDAD { get; set; }
        public DateTime FECHA_PROGRAMADA { get; set; }
        public decimal TIEMPO_TOTAL_TAREA { get; set; } // NUMBER(8,2)
        public string ESTADO { get; set; }              // VARCHAR2(20)
        public string ES_RECURRENTE { get; set; }       // V/F
        public int FRECUENCIA_DIAS { get; set; }        // NUMBER(4)
        public decimal COSTO_TRANSPORTE { get; set; }   // NUMBER(14,2)
    }
}
