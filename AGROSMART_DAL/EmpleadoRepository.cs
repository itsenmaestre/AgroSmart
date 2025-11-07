using AGROSMART_ENTITY.ENTIDADES;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_DAL
{
    public class EmpleadoRepository : BaseRepository<EMPLEADO>
    {
        public override string Guardar(EMPLEADO e)
        {
            const string sql = @"
                INSERT INTO EMPLEADO
                (ID_USUARIO, MONTO_POR_HORA, MONTO_POR_JORNAL)
                VALUES
                (:p_id, :p_hora, :p_jornal)";

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":p_id", OracleDbType.Int32).Value = e.ID_USUARIO;
                cmd.Parameters.Add(":p_hora", OracleDbType.Decimal).Value = e.MONTO_POR_HORA;
                cmd.Parameters.Add(":p_jornal", OracleDbType.Decimal).Value = e.MONTO_POR_JORNAL;

                cn.Open();
                return cmd.ExecuteNonQuery() == 1 ? "OK" : "No se insertó empleado";
            }
        }

        public override bool Actualizar(EMPLEADO e) => false;
        public override bool Eliminar(EMPLEADO e) => false;
        public override IList<EMPLEADO> Consultar()
        {
            return new List<EMPLEADO>();
        }
        public override EMPLEADO ObtenerPorId(int id) => null;
    }
}
