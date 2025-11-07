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
        // FECHA_PROGRAMADA por ID_TAREA
        public DateTime? ObtenerFechaProgramada(int idTarea)
        {
            const string sql = "SELECT FECHA_PROGRAMADA FROM TAREA WHERE ID_TAREA = :p";
            using (var cn = Conexion.CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":p", OracleDbType.Int32).Value = idTarea;
                cn.Open();
                var r = cmd.ExecuteScalar();
                if (r == null || r == DBNull.Value) return null;
                return Convert.ToDateTime(r);
            }
        }

        // Contar por estado (asignaciones del empleado)
        public int ContarPorEstado(int idEmpleado, string estado)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM ASIGNACION_TAREA a
                WHERE a.ID_EMPLEADO = :emp AND a.ESTADO = :est";
            using (var cn = Conexion.CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":emp", OracleDbType.Int32).Value = idEmpleado;
                cmd.Parameters.Add(":est", OracleDbType.Varchar2).Value = estado;
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Tareas programadas HOY
        public int ContarTareasDeHoy(int idEmpleado)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM ASIGNACION_TAREA a
                JOIN TAREA t ON t.ID_TAREA = a.ID_TAREA
                WHERE a.ID_EMPLEADO = :emp
                  AND TRUNC(t.FECHA_PROGRAMADA) = TRUNC(SYSDATE)";
            using (var cn = Conexion.CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":emp", OracleDbType.Int32).Value = idEmpleado;
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Vencidas (fecha < hoy y no finalizadas)
        public int ContarVencidas(int idEmpleado)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM ASIGNACION_TAREA a
                JOIN TAREA t ON t.ID_TAREA = a.ID_TAREA
                WHERE a.ID_EMPLEADO = :emp
                  AND TRUNC(t.FECHA_PROGRAMADA) < TRUNC(SYSDATE)
                  AND a.ESTADO <> 'FINALIZADA'";
            using (var cn = Conexion.CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":emp", OracleDbType.Int32).Value = idEmpleado;
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
