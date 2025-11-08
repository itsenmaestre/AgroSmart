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
    public class CosechaRepository : BaseRepository<COSECHA>
    {
        public override IList<COSECHA> Consultar()
        {
            const string sql = @"
                SELECT ID_COSECHA, ID_CULTIVO, ID_ADMIN_REGISTRO, 
                       FECHA_COSECHA, FECHA_REGISTRO, CANTIDAD_OBTENIDA,
                       UNIDAD_MEDIDA, CALIDAD, OBSERVACIONES
                FROM COSECHA
                ORDER BY FECHA_COSECHA DESC";

            List<COSECHA> lista = new List<COSECHA>();

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

        public override COSECHA ObtenerPorId(int id)
        {
            const string sql = @"
                SELECT ID_COSECHA, ID_CULTIVO, ID_ADMIN_REGISTRO, 
                       FECHA_COSECHA, FECHA_REGISTRO, CANTIDAD_OBTENIDA,
                       UNIDAD_MEDIDA, CALIDAD, OBSERVACIONES
                FROM COSECHA
                WHERE ID_COSECHA = :id";

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    return dr.Read() ? Mapear(dr) : null;
            }
        }

        public override string Guardar(COSECHA entidad)
        {
            const string sql = @"
                INSERT INTO COSECHA (ID_COSECHA, ID_CULTIVO, ID_ADMIN_REGISTRO, 
                                     FECHA_COSECHA, FECHA_REGISTRO, CANTIDAD_OBTENIDA,
                                     UNIDAD_MEDIDA, CALIDAD, OBSERVACIONES)
                VALUES (NULL, :cultivo, :admin, :fechaCos, :fechaReg, :cantidad, 
                        :unidad, :calidad, :obs)";

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":cultivo", OracleDbType.Int32).Value = entidad.ID_CULTIVO;
                cmd.Parameters.Add(":admin", OracleDbType.Int32).Value = entidad.ID_ADMIN_REGISTRO;
                cmd.Parameters.Add(":fechaCos", OracleDbType.Date).Value = entidad.FECHA_COSECHA;
                cmd.Parameters.Add(":fechaReg", OracleDbType.Date).Value = entidad.FECHA_REGISTRO;
                cmd.Parameters.Add(":cantidad", OracleDbType.Decimal).Value = entidad.CANTIDAD_OBTENIDA;
                cmd.Parameters.Add(":unidad", OracleDbType.Varchar2).Value = entidad.UNIDAD_MEDIDA;
                cmd.Parameters.Add(":calidad", OracleDbType.Varchar2).Value = entidad.CALIDAD;
                cmd.Parameters.Add(":obs", OracleDbType.Varchar2).Value = (object)entidad.OBSERVACIONES ?? DBNull.Value;

                cn.Open();
                return cmd.ExecuteNonQuery() == 1 ? "OK" : "No se insertó la cosecha";
            }
        }

        public override bool Actualizar(COSECHA entidad)
        {
            const string sql = @"
                UPDATE COSECHA
                SET ID_CULTIVO = :cultivo,
                    ID_ADMIN_REGISTRO = :admin,
                    FECHA_COSECHA = :fechaCos,
                    FECHA_REGISTRO = :fechaReg,
                    CANTIDAD_OBTENIDA = :cantidad,
                    UNIDAD_MEDIDA = :unidad,
                    CALIDAD = :calidad,
                    OBSERVACIONES = :obs
                WHERE ID_COSECHA = :id";

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":cultivo", OracleDbType.Int32).Value = entidad.ID_CULTIVO;
                cmd.Parameters.Add(":admin", OracleDbType.Int32).Value = entidad.ID_ADMIN_REGISTRO;
                cmd.Parameters.Add(":fechaCos", OracleDbType.Date).Value = entidad.FECHA_COSECHA;
                cmd.Parameters.Add(":fechaReg", OracleDbType.Date).Value = entidad.FECHA_REGISTRO;
                cmd.Parameters.Add(":cantidad", OracleDbType.Decimal).Value = entidad.CANTIDAD_OBTENIDA;
                cmd.Parameters.Add(":unidad", OracleDbType.Varchar2).Value = entidad.UNIDAD_MEDIDA;
                cmd.Parameters.Add(":calidad", OracleDbType.Varchar2).Value = entidad.CALIDAD;
                cmd.Parameters.Add(":obs", OracleDbType.Varchar2).Value = (object)entidad.OBSERVACIONES ?? DBNull.Value;
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = entidad.ID_COSECHA;

                cn.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public override bool Eliminar(COSECHA entidad)
        {
            const string sql = "DELETE FROM COSECHA WHERE ID_COSECHA = :id";

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = entidad.ID_COSECHA;
                cn.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public List<COSECHA> ObtenerPorCultivo(int idCultivo)
        {
            const string sql = @"
                SELECT ID_COSECHA, ID_CULTIVO, ID_ADMIN_REGISTRO, 
                       FECHA_COSECHA, FECHA_REGISTRO, CANTIDAD_OBTENIDA,
                       UNIDAD_MEDIDA, CALIDAD, OBSERVACIONES
                FROM COSECHA
                WHERE ID_CULTIVO = :id
                ORDER BY FECHA_COSECHA DESC";

            List<COSECHA> lista = new List<COSECHA>();

            using (var cn = CrearConexion())
            using (var cmd = new OracleCommand(sql, cn))
            {
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = idCultivo;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(Mapear(dr));
                }
            }
            return lista;
        }

        private COSECHA Mapear(OracleDataReader dr)
        {
            return new COSECHA
            {
                ID_COSECHA = Convert.ToInt32(dr["ID_COSECHA"]),
                ID_CULTIVO = Convert.ToInt32(dr["ID_CULTIVO"]),
                ID_ADMIN_REGISTRO = Convert.ToInt32(dr["ID_ADMIN_REGISTRO"]),
                FECHA_COSECHA = Convert.ToDateTime(dr["FECHA_COSECHA"]),
                FECHA_REGISTRO = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                CANTIDAD_OBTENIDA = Convert.ToDecimal(dr["CANTIDAD_OBTENIDA"]),
                UNIDAD_MEDIDA = dr["UNIDAD_MEDIDA"].ToString(),
                CALIDAD = dr["CALIDAD"].ToString(),
                OBSERVACIONES = dr["OBSERVACIONES"] == DBNull.Value ? null : dr["OBSERVACIONES"].ToString()
            };
        }
    }
}
