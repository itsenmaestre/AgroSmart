using AGROSMART_ENTITY.ENTIDADES;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_DAL
{
    public class AsignacionTareaRepository
    {
        public List<ASIGNACION_TAREA> ListarPorEmpleado(int idEmpleado)
        {
            List<ASIGNACION_TAREA> lista = new List<ASIGNACION_TAREA>();

            string sql = "SELECT * FROM ASIGNACION_TAREA WHERE ID_EMPLEADO = :id";

            using (OracleConnection cn = Conexion.CrearConexion())
            using (OracleCommand cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = idEmpleado;
                cn.Open();
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ASIGNACION_TAREA a = new ASIGNACION_TAREA();
                    a.ID_ASIG_TAREA = Convert.ToInt32(dr["ID_ASIG_TAREA"]);
                    a.ID_TAREA = Convert.ToInt32(dr["ID_TAREA"]);
                    a.ID_EMPLEADO = Convert.ToInt32(dr["ID_EMPLEADO"]);
                    a.ESTADO = dr["ESTADO"].ToString();

                    if (dr["HORAS_TRABAJADAS"] != DBNull.Value)
                        a.HORAS_TRABAJADAS = Convert.ToDecimal(dr["HORAS_TRABAJADAS"]);

                    if (dr["JORNADAS_TRABAJADAS"] != DBNull.Value)
                        a.JORNADAS_TRABAJADAS = Convert.ToDecimal(dr["JORNADAS_TRABAJADAS"]);

                    if (dr["PAGO_ACORDADO"] != DBNull.Value)
                        a.PAGO_ACORDADO = Convert.ToDecimal(dr["PAGO_ACORDADO"]);

                    lista.Add(a);
                }
                dr.Close();
            }

            return lista;
        }

        public string ActualizarAvance(ASIGNACION_TAREA a)
        {
            string sql = @"UPDATE ASIGNACION_TAREA
                           SET HORAS_TRABAJADAS = :horas,
                               JORNADAS_TRABAJADAS = :jornadas,
                               ESTADO = :estado
                           WHERE ID_ASIG_TAREA = :id";

            using (OracleConnection cn = Conexion.CrearConexion())
            using (OracleCommand cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":horas", OracleDbType.Decimal).Value = a.HORAS_TRABAJADAS;
                cmd.Parameters.Add(":jornadas", OracleDbType.Decimal).Value = a.JORNADAS_TRABAJADAS;
                cmd.Parameters.Add(":estado", OracleDbType.Varchar2).Value = a.ESTADO;
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = a.ID_ASIG_TAREA;

                cn.Open();
                int filas = cmd.ExecuteNonQuery();
                return filas == 1 ? "OK" : "No se actualizó el avance.";
            }
        }
    }
}
