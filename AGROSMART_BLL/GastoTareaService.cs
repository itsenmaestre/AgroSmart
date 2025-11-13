using AGROSMART_DAL;
using AGROSMART_ENTITY.ENTIDADES;
using AGROSMART_ENTITY.ENTIDADES_DTOS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AGROSMART_BLL
{
    public class GastoTareaService
    {
        private readonly TareaService _tareaService = new TareaService();
        private readonly CultivoService _cultivoService = new CultivoService();
        private readonly DetalleTareaService _detalleService = new DetalleTareaService();
        private readonly InsumoService _insumoService = new InsumoService();
        private readonly AsignacionTareaService _asignacionService = new AsignacionTareaService();
        private readonly EmpleadoService _empleadoService = new EmpleadoService();
        private readonly UsuarioService _usuarioService = new UsuarioService();

        private static readonly string[] EstadosFinalizados = new[] { "COMPLETADA", "FINALIZADA", "TERMINADA" };

        public List<GastoTareaDTO> ObtenerGastosTareasFinalizadas()
        {
            var tareas = new List<TAREA>();
            foreach (var estado in EstadosFinalizados)
            {
                try
                {
                    var tareasEstado = _tareaService.ObtenerPorEstado(estado) ?? new List<TAREA>();
                    foreach (var tarea in tareasEstado)
                    {
                        if (!tareas.Any(t => t.ID_TAREA == tarea.ID_TAREA))
                        {
                            tareas.Add(tarea);
                        }
                    }
                }
                catch
                {
                    // Si el repositorio no soporta el estado, se ignora silenciosamente.
                }
            }

            var resultado = new List<GastoTareaDTO>();

            foreach (var tarea in tareas)
            {
                if (string.IsNullOrWhiteSpace(tarea.ESTADO) ||
                    !EstadosFinalizados.Contains(tarea.ESTADO.ToUpperInvariant()))
                {
                    continue;
                }

                var dto = new GastoTareaDTO
                {
                    IdTarea = tarea.ID_TAREA,
                    TipoActividad = tarea.TIPO_ACTIVIDAD,
                    FechaProgramada = tarea.FECHA_PROGRAMADA,
                    Estado = tarea.ESTADO ?? string.Empty,
                    CostoTransporte = tarea.COSTO_TRANSPORTE
                };

                var cultivo = _cultivoService.ObtenerPorId(tarea.ID_CULTIVO);
                dto.NombreCultivo = cultivo != null ? cultivo.NOMBRE_LOTE : "Sin cultivo";

                dto.DetallesInsumos = ObtenerGastoInsumos(tarea.ID_TAREA);
                dto.TotalInsumos = dto.DetallesInsumos.Sum(i => i.CostoTotal);

                dto.DetallesEmpleados = ObtenerGastoEmpleados(tarea.ID_TAREA);
                dto.TotalManoObra = dto.DetallesEmpleados.Sum(e => e.MontoCalculado);

                dto.TotalGasto = dto.TotalInsumos + dto.TotalManoObra + dto.CostoTransporte;

                resultado.Add(dto);
            }

            return resultado
                .OrderByDescending(r => r.FechaProgramada)
                .ThenByDescending(r => r.IdTarea)
                .ToList();
        }

        private List<GastoInsumoDTO> ObtenerGastoInsumos(int idTarea)
        {
            var lista = new List<GastoInsumoDTO>();
            try
            {
                var detalles = _detalleService.ObtenerPorTarea(idTarea);
                foreach (var det in detalles)
                {
                    var insumo = _insumoService.ObtenerPorId(det.ID_INSUMO);
                    if (insumo == null)
                        continue;

                    decimal costoUnitario = insumo.COSTO_UNITARIO;
                    decimal costoTotal = costoUnitario * det.CANTIDAD_USADA;

                    lista.Add(new GastoInsumoDTO
                    {
                        IdDetalle = det.ID_DETALLE_TAREA,
                        IdTarea = det.ID_TAREA,
                        IdInsumo = det.ID_INSUMO,
                        NombreInsumo = insumo.NOMBRE,
                        TipoInsumo = insumo.TIPO,
                        CantidadUsada = det.CANTIDAD_USADA,
                        CostoUnitario = costoUnitario,
                        CostoTotal = Math.Round(costoTotal, 2)
                    });
                }
            }
            catch
            {
                // Si ocurre algún error, se retorna la lista acumulada hasta el momento
            }

            return lista;
        }

        private List<GastoEmpleadoDTO> ObtenerGastoEmpleados(int idTarea)
        {
            var lista = new List<GastoEmpleadoDTO>();
            try
            {
                var asignaciones = _asignacionService.ListarPorTarea(idTarea);
                foreach (var asignacion in asignaciones)
                {
                    if (string.IsNullOrWhiteSpace(asignacion.ESTADO) ||
                        !EstadosFinalizados.Contains(asignacion.ESTADO.ToUpperInvariant()))
                    {
                        continue;
                    }

                    var empleado = _empleadoService.ObtenerPorId(asignacion.ID_EMPLEADO);
                    var usuario = _usuarioService.ObtenerPorId(asignacion.ID_EMPLEADO);

                    decimal monto = asignacion.PAGO_ACORDADO ?? 0m;
                    if (monto <= 0m && empleado != null)
                    {
                        if (asignacion.JORNADAS_TRABAJADAS.HasValue && asignacion.JORNADAS_TRABAJADAS.Value > 0)
                        {
                            monto = asignacion.JORNADAS_TRABAJADAS.Value * empleado.MONTO_POR_JORNAL;
                        }
                        else if (asignacion.HORAS_TRABAJADAS.HasValue && asignacion.HORAS_TRABAJADAS.Value > 0)
                        {
                            monto = asignacion.HORAS_TRABAJADAS.Value * empleado.MONTO_POR_HORA;
                        }
                    }

                    lista.Add(new GastoEmpleadoDTO
                    {
                        IdAsignacion = asignacion.ID_ASIG_TAREA,
                        IdTarea = asignacion.ID_TAREA,
                        IdEmpleado = asignacion.ID_EMPLEADO,
                        NombreEmpleado = usuario != null
                            ? string.Join(" ", new[]
                                {
                                    usuario.PRIMER_NOMBRE,
                                    usuario.SEGUNDO_NOMBRE,
                                    usuario.PRIMER_APELLIDO,
                                    usuario.SEGUNDO_APELLIDO
                                }.Where(n => !string.IsNullOrWhiteSpace(n)))
                            : $"Empleado #{asignacion.ID_EMPLEADO}",
                        JornadasTrabajadas = asignacion.JORNADAS_TRABAJADAS ?? 0m,
                        HorasTrabajadas = asignacion.HORAS_TRABAJADAS ?? 0m,
                        PagoAcordado = asignacion.PAGO_ACORDADO,
                        MontoCalculado = Math.Round(monto, 2)
                    });
                }
            }
            catch
            {
                // Ignorar errores y retornar la lista acumulada
            }

            return lista;
        }
    }
}
