using AGROSMART_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_BLL
{
    public class TareaService
    {
        private readonly TareaRepository _repo = new TareaRepository();

        public DateTime? ObtenerFechaProgramada(int idTarea) => _repo.ObtenerFechaProgramada(idTarea);
        public int ContarPorEstado(int idEmpleado, string estado) => _repo.ContarPorEstado(idEmpleado, estado);
        public int ContarTareasDeHoy(int idEmpleado) => _repo.ContarTareasDeHoy(idEmpleado);
        public int ContarVencidas(int idEmpleado) => _repo.ContarVencidas(idEmpleado);
    }
}
