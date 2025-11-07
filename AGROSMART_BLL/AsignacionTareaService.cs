using AGROSMART_DAL;
using AGROSMART_ENTITY.ENTIDADES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_BLL
{
    public class AsignacionTareaService
    {
        AsignacionTareaRepository repo = new AsignacionTareaRepository();

        public List<ASIGNACION_TAREA> ListarPorEmpleado(int idEmpleado)
        {
            return repo.ListarPorEmpleado(idEmpleado);
        }

        public string ActualizarAvance(ASIGNACION_TAREA a)
        {
            return repo.ActualizarAvance(a);
        }
    }
}
