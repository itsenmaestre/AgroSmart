using AGROSMART_DAL;
using AGROSMART_ENTITY.ENTIDADES;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_BLL
{
    public class CosechaService : ICrudLectura<COSECHA>, ICrudEscritura<COSECHA>
    {
        private readonly CosechaRepository _repo = new CosechaRepository();

        public ReadOnlyCollection<COSECHA> Consultar()
        {
            return new ReadOnlyCollection<COSECHA>(_repo.Consultar().ToList());
        }

        public COSECHA ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la cosecha debe ser mayor a cero.");

            return _repo.ObtenerPorId(id);
        }

        public string Guardar(COSECHA entidad)
        {
            // Validaciones
            if (entidad == null)
                throw new ArgumentNullException(nameof(entidad));

            if (entidad.ID_CULTIVO <= 0)
                throw new ArgumentException("Debe especificar un cultivo válido.");

            if (entidad.ID_ADMIN_REGISTRO <= 0)
                throw new ArgumentException("Debe especificar un administrador válido.");

            if (entidad.CANTIDAD_OBTENIDA < 0)
                throw new ArgumentException("La cantidad obtenida no puede ser negativa.");

            if (string.IsNullOrWhiteSpace(entidad.UNIDAD_MEDIDA))
                throw new ArgumentException("La unidad de medida es obligatoria.");

            if (string.IsNullOrWhiteSpace(entidad.CALIDAD))
                throw new ArgumentException("La calidad es obligatoria.");

            if (entidad.FECHA_REGISTRO < entidad.FECHA_COSECHA)
                throw new ArgumentException("La fecha de registro no puede ser anterior a la fecha de cosecha.");

            return _repo.Guardar(entidad);
        }

        public bool Actualizar(COSECHA entidad)
        {
            // Validaciones
            if (entidad == null)
                throw new ArgumentNullException(nameof(entidad));

            if (entidad.ID_COSECHA <= 0)
                throw new ArgumentException("El ID de la cosecha es inválido.");

            if (entidad.CANTIDAD_OBTENIDA < 0)
                throw new ArgumentException("La cantidad obtenida no puede ser negativa.");

            if (string.IsNullOrWhiteSpace(entidad.UNIDAD_MEDIDA))
                throw new ArgumentException("La unidad de medida es obligatoria.");

            return _repo.Actualizar(entidad);
        }

        public bool Eliminar(COSECHA entidad)
        {
            if (entidad == null || entidad.ID_COSECHA <= 0)
                throw new ArgumentException("Cosecha inválida para eliminar.");

            return _repo.Eliminar(entidad);
        }

        // Métodos adicionales
        public List<COSECHA> ObtenerPorCultivo(int idCultivo)
        {
            if (idCultivo <= 0)
                throw new ArgumentException("ID de cultivo inválido.");

            return _repo.ObtenerPorCultivo(idCultivo);
        }

        public decimal ObtenerTotalCosechadoPorCultivo(int idCultivo)
        {
            var cosechas = _repo.ObtenerPorCultivo(idCultivo);
            return cosechas.Sum(c => c.CANTIDAD_OBTENIDA);
        }
    }
}
