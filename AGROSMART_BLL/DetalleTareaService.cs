using AGROSMART_DAL;
using AGROSMART_ENTITY.ENTIDADES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_BLL
{
    public class DetalleTareaService
    {
        private readonly DetalleTareaRepository _repo = new DetalleTareaRepository();

        public List<DETALLE_TAREA> ObtenerPorTarea(int idTarea)
        {
            if (idTarea <= 0)
                throw new ArgumentException("ID de tarea inválido.");

            return _repo.ObtenerPorTarea(idTarea);
        }

        public string RegistrarInsumosConDescuento(int idTarea, List<DETALLE_TAREA> detalles)
        {
            if (idTarea <= 0)
                throw new ArgumentException("ID de tarea inválido.");

            if (detalles == null || detalles.Count == 0)
                throw new ArgumentException("Debe especificar al menos un insumo.");

            // Validar cantidades
            foreach (var det in detalles)
            {
                if (det.CANTIDAD_USADA < 0)
                    throw new ArgumentException("Las cantidades no pueden ser negativas.");
            }

            return _repo.RegistrarInsumosConDescuento(idTarea, detalles);
        }
    }
}
