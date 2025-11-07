using AGROSMART_ENTITY.ENTIDADES_DTOS;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_DAL
{
    public class TareaRepository
    {
        public IList<AsignacionEmpleadoDTO> TareasAsignadasConInfo(int idEmpleado)
        {
            const string sql = @"
                SELECT 
                  a.ID_ASIG_TAREA, a.ID_TAREA, a.ID_EMPLEADO,
                  a.ESTADO AS ESTADO_ASIG,
                  a.HORAS_TRABAJADAS, a.JORNADAS_TRABAJADAS, a.PAGO_ACORDADO,
                  t.TIPO_ACTIVIDAD, t.FECHA_PROGRAMADA, t.ESTADO AS ESTADO_TAREA
                FROM ASIGNACION_TAREA a
                JOIN TAREA t ON t.ID_TAREA = a.ID_TAREA
                WHERE a.ID_EMPLEADO = :p_emp
                ORDER BY t.FECHA_PROGRAMADA DESC";

            List<AsignacionEmpleadoDTO> lista = new List<AsignacionEmpleadoDTO>();

            using (OracleConnection cn = Conexion.CrearConexion())
            using (OracleCommand cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":p_emp", OracleDbType.Int32).Value = idEmpleado;
                cn.Open();

                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        AsignacionEmpleadoDTO dto = new AsignacionEmpleadoDTO();
                        dto.ID_ASIG_TAREA = dr.GetInt32(dr.GetOrdinal("ID_ASIG_TAREA"));
                        dto.ID_TAREA = dr.GetInt32(dr.GetOrdinal("ID_TAREA"));
                        dto.ID_EMPLEADO = dr.GetInt32(dr.GetOrdinal("ID_EMPLEADO"));
                        dto.ESTADO_ASIGNACION = dr["ESTADO_ASIG"] as string;

                        int ixH = dr.GetOrdinal("HORAS_TRABAJADAS");
                        dto.HORAS_TRABAJADAS = dr.IsDBNull(ixH) ? (decimal?)null : dr.GetDecimal(ixH);

                        int ixJ = dr.GetOrdinal("JORNADAS_TRABAJADAS");
                        dto.JORNADAS_TRABAJADAS = dr.IsDBNull(ixJ) ? (decimal?)null : dr.GetDecimal(ixJ);

                        int ixP = dr.GetOrdinal("PAGO_ACORDADO");
                        dto.PAGO_ACORDADO = dr.IsDBNull(ixP) ? (decimal?)null : dr.GetDecimal(ixP);

                        dto.TIPO_ACTIVIDAD = dr["TIPO_ACTIVIDAD"] as string;
                        dto.FECHA_PROGRAMADA = dr.GetDateTime(dr.GetOrdinal("FECHA_PROGRAMADA"));
                        dto.ESTADO_TAREA = dr["ESTADO_TAREA"] as string;

                        lista.Add(dto);
                    }
                }
            }
            return lista;
        }
    }
}
