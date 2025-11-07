using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_DAL
{
    public abstract class BaseRepository<T>
    {
        protected OracleConnection CrearConexion() => Conexion.CrearConexion();

        // El profe devuelve string en Guardar; aquí lo dejamos virtual para sobreescribirlo.
        public virtual string Guardar(T entidad) => "No implementado";
        public virtual bool Actualizar(T entidad) => false;
        public virtual bool Eliminar(T entidad) => false;

        public abstract IList<T> Consultar();
        public abstract T ObtenerPorId(int id);
    }
}
