using AGROSMART_ENTITY.ENTIDADES;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGROSMART_DAL
{
    public class DetalleTareaRepository : BaseRepository<DETALLE_TAREA>
    {
        public override IList<DETALLE_TAREA> Consultar()
        {
            const string sql = @"
                SELECT ID_DETALLE_TAREA, ID_TAREA, ID_INSUMO, CANTIDAD_USADA
                FROM DETALLE_TAREA
                ORDER BY ID_DETALLE_TAREA DESC";

            List<DETALLE_TAREA> lista = new List<DETALLE_TAREA>();

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(Mapear(dr));
                }
            }
            return lista;
        }

        public override DETALLE_TAREA ObtenerPorId(int id)
        {
            const string sql = @"
                SELECT ID_DETALLE_TAREA, ID_TAREA, ID_INSUMO, CANTIDAD_USADA
                FROM DETALLE_TAREA
                WHERE ID_DETALLE_TAREA = :id";

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    return dr.Read() ? Mapear(dr) : null;
            }
        }

        public override string Guardar(DETALLE_TAREA entidad)
        {
            const string sql = @"
                INSERT INTO DETALLE_TAREA (ID_TAREA, ID_INSUMO, CANTIDAD_USADA)
                VALUES (:tarea, :insumo, :cantidad)";

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":tarea", OracleDbType.Int32).Value = entidad.ID_TAREA;
                cmd.Parameters.Add(":insumo", OracleDbType.Int32).Value = entidad.ID_INSUMO;
                cmd.Parameters.Add(":cantidad", OracleDbType.Decimal).Value = entidad.CANTIDAD_USADA;

                cn.Open();
                return cmd.ExecuteNonQuery() == 1 ? "OK" : "No se insertó el detalle";
            }
        }

        public override bool Actualizar(DETALLE_TAREA entidad)
        {
            const string sql = @"
                UPDATE DETALLE_TAREA
                SET ID_TAREA = :tarea,
                    ID_INSUMO = :insumo,
                    CANTIDAD_USADA = :cantidad
                WHERE ID_DETALLE_TAREA = :id";

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":tarea", OracleDbType.Int32).Value = entidad.ID_TAREA;
                cmd.Parameters.Add(":insumo", OracleDbType.Int32).Value = entidad.ID_INSUMO;
                cmd.Parameters.Add(":cantidad", OracleDbType.Decimal).Value = entidad.CANTIDAD_USADA;
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = entidad.ID_DETALLE_TAREA;

                cn.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public override bool Eliminar(DETALLE_TAREA entidad)
        {
            const string sql = "DELETE FROM DETALLE_TAREA WHERE ID_DETALLE_TAREA = :id";

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = entidad.ID_DETALLE_TAREA;
                cn.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        /// <summary>
        /// Obtiene todos los insumos usados en una tarea específica
        /// </summary>
        public List<DETALLE_TAREA> ObtenerPorTarea(int idTarea)
        {
            const string sql = @"
                SELECT ID_DETALLE_TAREA, ID_TAREA, ID_INSUMO, CANTIDAD_USADA
                FROM DETALLE_TAREA
                WHERE ID_TAREA = :id";

            List<DETALLE_TAREA> lista = new List<DETALLE_TAREA>();

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = idTarea;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(Mapear(dr));
                }
            }
            return lista;
        }

        /// <summary>
        /// Registra múltiples insumos para una tarea y descuenta stock (RN-43, RN-44)
        /// </summary>
        public string RegistrarInsumosConDescuento(int idTarea, List<DETALLE_TAREA> detalles)
        {
            using (var cn = CrearConexion())
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        foreach (var detalle in detalles)
                        {
                            // Validar stock disponible (RN-44)
                            const string sqlCheck = "SELECT STOCK_ACTUAL, TIPO FROM INSUMO WHERE ID_INSUMO = :id";
                            decimal stockActual;
                            string tipo;

                            using (var cmdCheck = new OracleCommand(sqlCheck, cn))
                            {
                                cmdCheck.Transaction = tx;
                                cmdCheck.Parameters.Add(":id", OracleDbType.Int32).Value = detalle.ID_INSUMO;

                                using (var dr = cmdCheck.ExecuteReader())
                                {
                                    if (!dr.Read())
                                        throw new Exception($"Insumo {detalle.ID_INSUMO} no encontrado");

                                    stockActual = Convert.ToDecimal(dr["STOCK_ACTUAL"]);
                                    tipo = dr["TIPO"].ToString();
                                }
                            }

                            if (detalle.CANTIDAD_USADA > stockActual)
                                throw new Exception($"Stock insuficiente para insumo {detalle.ID_INSUMO}. Disponible: {stockActual}");

                            // Insertar detalle
                            const string sqlInsert = @"
                                INSERT INTO DETALLE_TAREA (ID_TAREA, ID_INSUMO, CANTIDAD_USADA)
                                VALUES (:tarea, :insumo, :cantidad)";

                            using (var cmdInsert = new OracleCommand(sqlInsert, cn))
                            {
                                cmdInsert.Transaction = tx;
                                cmdInsert.Parameters.Add(":tarea", OracleDbType.Int32).Value = idTarea;
                                cmdInsert.Parameters.Add(":insumo", OracleDbType.Int32).Value = detalle.ID_INSUMO;
                                cmdInsert.Parameters.Add(":cantidad", OracleDbType.Decimal).Value = detalle.CANTIDAD_USADA;
                                cmdInsert.ExecuteNonQuery();
                            }

                            // Descontar stock solo si es CONSUMIBLE (RN-24)
                            if (tipo == "CONSUMIBLE")
                            {
                                const string sqlUpdate = @"
                                    UPDATE INSUMO 
                                    SET STOCK_ACTUAL = STOCK_ACTUAL - :cantidad
                                    WHERE ID_INSUMO = :id";

                                using (var cmdUpdate = new OracleCommand(sqlUpdate, cn))
                                {
                                    cmdUpdate.Transaction = tx;
                                    cmdUpdate.Parameters.Add(":cantidad", OracleDbType.Decimal).Value = detalle.CANTIDAD_USADA;
                                    cmdUpdate.Parameters.Add(":id", OracleDbType.Int32).Value = detalle.ID_INSUMO;
                                    cmdUpdate.ExecuteNonQuery();
                                }
                            }
                        }

                        tx.Commit();
                        return "OK";
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        return $"Error: {ex.Message}";
                    }
                }
            }
        }

        private DETALLE_TAREA Mapear(OracleDataReader dr)
        {
            return new DETALLE_TAREA
            {
                ID_DETALLE_TAREA = Convert.ToInt32(dr["ID_DETALLE_TAREA"]),
                ID_TAREA = Convert.ToInt32(dr["ID_TAREA"]),
                ID_INSUMO = Convert.ToInt32(dr["ID_INSUMO"]),
                CANTIDAD_USADA = Convert.ToDecimal(dr["CANTIDAD_USADA"])
            };
        }
    }
}
