using AGROSMART_BLL;
using AGROSMART_ENTITY.ENTIDADES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AGROSMART_GUI.Views.Admin
{
    /// <summary>
    /// Lógica de interacción para AsignarEmpleadosPage.xaml
    /// </summary>
    public partial class AsignarEmpleadosPage : Page
    {
        private readonly AsignacionTareaService _asigService = new AsignacionTareaService();
        private readonly TareaService _tareaService = new TareaService();
        private readonly EmpleadoService _empleadoService = new EmpleadoService();
        private readonly int _idAdmin;

        public AsignarEmpleadosPage(int idAdmin)
        {
            InitializeComponent();
            _idAdmin = idAdmin;

            // Establecer fecha por defecto
            dpFechaAsignacion.SelectedDate = DateTime.Now;

            CargarDatos();
            CargarAsignaciones();
        }

        private void CargarDatos()
        {
            try
            {
                // Cargar tareas disponibles
                var tareas = _tareaService.Consultar();
                var tareasVM = tareas.Select(t => new
                {
                    IdTarea = t.ID_TAREA,
                    Display = $"#{t.ID_TAREA} - {t.TIPO_ACTIVIDAD} ({t.ESTADO})"
                }).ToList();
                cboTarea.ItemsSource = tareasVM;

                // Cargar empleados
                var empleados = _empleadoService.ListarEmpleadosConUsuario();
                var empleadosVM = empleados.Select(e => new
                {
                    IdUsuario = e.IdUsuario,
                    Display = $"{e.NombreCompleto} - ${e.MontoPorHora:N2}/h"
                }).ToList();
                cboEmpleado.ItemsSource = empleadosVM;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CboTarea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboTarea.SelectedValue != null)
            {
                try
                {
                    int idTarea = Convert.ToInt32(cboTarea.SelectedValue);
                    var tarea = _tareaService.ObtenerPorId(idTarea);

                    if (tarea != null)
                    {
                        txtDetallesTarea.Text = $"Actividad: {tarea.TIPO_ACTIVIDAD}\n" +
                                              $"Fecha programada: {tarea.FECHA_PROGRAMADA:dd/MM/yyyy}\n" +
                                              $"Tiempo estimado: {tarea.TIEMPO_TOTAL_TAREA:N2} horas\n" +
                                              $"Estado actual: {tarea.ESTADO}";
                    }
                }
                catch (Exception ex)
                {
                    txtDetallesTarea.Text = "Error al cargar detalles de la tarea";
                }
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            cboTarea.SelectedIndex = -1;
            cboEmpleado.SelectedIndex = -1;
            dpFechaAsignacion.SelectedDate = DateTime.Now;
            txtPagoAcordado.Text = "0.00";
            txtDetallesTarea.Text = "Selecciona una tarea para ver sus detalles";
        }

        private void BtnAsignar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validaciones
                if (cboTarea.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar una tarea.", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cboEmpleado.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un empleado.", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!dpFechaAsignacion.SelectedDate.HasValue)
                {
                    MessageBox.Show("Debe seleccionar la fecha de asignación.", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int idTarea = Convert.ToInt32(cboTarea.SelectedValue);
                int idEmpleado = Convert.ToInt32(cboEmpleado.SelectedValue);

                decimal pagoAcordado = 0;
                if (!string.IsNullOrWhiteSpace(txtPagoAcordado.Text))
                {
                    if (!decimal.TryParse(txtPagoAcordado.Text, out pagoAcordado) || pagoAcordado < 0)
                    {
                        MessageBox.Show("El pago acordado debe ser un número válido mayor o igual a 0.", "Validación",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                var asignacion = new ASIGNACION_TAREA
                {
                    ID_TAREA = idTarea,
                    ID_EMPLEADO = idEmpleado,
                    ID_ADMIN_ASIGNADOR = _idAdmin,
                    FECHA_ASIGNACION = dpFechaAsignacion.SelectedDate.Value,
                    ESTADO = "PENDIENTE",
                    PAGO_ACORDADO = pagoAcordado > 0 ? pagoAcordado : (decimal?)null
                };

                string resultado = _asigService.Asignar(asignacion);

                if (resultado == "OK")
                {
                    MessageBox.Show("Tarea asignada exitosamente.", "Éxito",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    BtnLimpiar_Click(sender, e);
                    CargarAsignaciones();
                }
                else
                {
                    MessageBox.Show($"Error al asignar: {resultado}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarAsignaciones()
        {
            try
            {
                var asignaciones = _asigService.ListarTodas();
                var empleadosInfo = _empleadoService.ListarEmpleadosConUsuario();

                var viewModels = asignaciones.Select(a =>
                {
                    var empleado = empleadosInfo.FirstOrDefault(e => e.IdUsuario == a.ID_EMPLEADO);
                    var tarea = _tareaService.ObtenerPorId(a.ID_TAREA);

                    return new AsignacionViewModel
                    {
                        IdAsignacion = a.ID_ASIG_TAREA,
                        NombreTarea = tarea != null ? $"#{a.ID_TAREA} - {tarea.TIPO_ACTIVIDAD}" : $"Tarea #{a.ID_TAREA}",
                        NombreEmpleado = empleado?.NombreCompleto ?? "Empleado desconocido",
                        FechaAsignacion = a.FECHA_ASIGNACION,
                        Estado = a.ESTADO,
                        PagoAcordado = a.PAGO_ACORDADO ?? 0
                    };
                }).ToList();

                dgAsignaciones.ItemsSource = viewModels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar asignaciones: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            CargarAsignaciones();
        }

        private void BtnVerDetalle_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.Tag is AsignacionViewModel vm)
            {
                string mensaje = $"ID Asignación: {vm.IdAsignacion}\n" +
                               $"Tarea: {vm.NombreTarea}\n" +
                               $"Empleado: {vm.NombreEmpleado}\n" +
                               $"Fecha: {vm.FechaAsignacion:dd/MM/yyyy}\n" +
                               $"Estado: {vm.Estado}\n" +
                               $"Pago Acordado: ${vm.PagoAcordado:N2}";

                MessageBox.Show(mensaje, "Detalles de la Asignación",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.Tag is AsignacionViewModel vm)
            {
                var result = MessageBox.Show(
                    $"¿Está seguro de cancelar la asignación #{vm.IdAsignacion}?",
                    "Confirmar Cancelación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        bool cancelado = _asigService.CancelarAsignacion(vm.IdAsignacion);
                        if (cancelado)
                        {
                            MessageBox.Show("Asignación cancelada exitosamente.", "Éxito",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            CargarAsignaciones();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo cancelar la asignación.", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public class AsignacionViewModel
        {
            public int IdAsignacion { get; set; }
            public string NombreTarea { get; set; }
            public string NombreEmpleado { get; set; }
            public DateTime FechaAsignacion { get; set; }
            public string Estado { get; set; }
            public decimal PagoAcordado { get; set; }
        }
    }
}
