using AGROSMART_BLL;
using AGROSMART_ENTITY.ENTIDADES;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AGROSMART_GUI.Views.Empleado
{
    /// <summary>
    /// Lógica de interacción para EmpleadoView.xaml
    /// </summary>
    public partial class EmpleadoView : Window
    {
        // Services en estilo simple (sin ADO.NET aquí)
        private readonly AsignacionTareaService _asigService = new AsignacionTareaService();
        private readonly TareaService _tareaService = new TareaService();

        private readonly int _idEmpleado;
        private string _nombreEmpleado;
        private List<ASIGNACION_TAREA> _asignaciones; // cache para buscar por ID_TAREA

        // Clase simple para bindear ItemsControl (coincide con el XAML de Claude)
        private class TareaItem
        {
            public int Codigo { get; set; }              // ID_TAREA
            public string Estado { get; set; }           // ESTADO (ASIGNACION_TAREA)
            public string FechaProgramada { get; set; }  // "dd/MM/yyyy" o "-"
            public string HorasAcumuladas { get; set; }  // "n.nn" o "-"
            public int IdTarea { get; set; }             // para Tag en botón "Registrar avance"
        }

        // Firma compatible con el Login mejorado de Claude (id + nombre)
        public EmpleadoView(int idEmpleadoActual, string nombreCompleto = null)
        {
            InitializeComponent();

            _idEmpleado = idEmpleadoActual;
            _nombreEmpleado = nombreCompleto;

            if (!string.IsNullOrWhiteSpace(_nombreEmpleado))
                txtBienvenida.Text = "Hola, " + _nombreEmpleado;

            CargarDatosIniciales();
        }

        // ====== Carga de tablero ======
        private void CargarDatosIniciales()
        {
            try
            {
                // 1) Traer asignaciones reales de Oracle (ASIGNACION_TAREA del empleado)
                _asignaciones = _asigService.ListarPorEmpleado(_idEmpleado);

                // 2) Convertir a objetos que espera el XAML de Claude
                List<TareaItem> todos = new List<TareaItem>();
                foreach (ASIGNACION_TAREA a in _asignaciones)
                {
                    // Fecha programada (TAREA) desde servicio sencillo
                    var fecha = _tareaService.ObtenerFechaProgramada(a.ID_TAREA);

                    TareaItem ti = new TareaItem();
                    ti.Codigo = a.ID_TAREA;
                    ti.Estado = a.ESTADO;
                    ti.FechaProgramada = fecha.HasValue ? fecha.Value.ToString("dd/MM/yyyy") : "-";
                    ti.HorasAcumuladas = a.HORAS_TRABAJADAS.HasValue ? a.HORAS_TRABAJADAS.Value.ToString("0.##") : "-";
                    ti.IdTarea = a.ID_TAREA;

                    todos.Add(ti);
                }

                // 3) Pintar "Mis tareas"
                lstMisTareas.ItemsSource = todos;

                // 4) Pintar "Ordenar" (pendientes: no FINALIZADA)
                var pendientes = todos.Where(t => !string.Equals(t.Estado, "FINALIZADA", StringComparison.OrdinalIgnoreCase)).ToList();
                lstTareasPendientes.ItemsSource = pendientes;

                // 5) Tarjetas (estadísticas reales)
                txtTareasHoy.Text = _tareaService.ContarTareasDeHoy(_idEmpleado).ToString();
                txtEnProgreso.Text = _tareaService.ContarPorEstado(_idEmpleado, "EN_EJECUCION").ToString();
                txtVencidas.Text = _tareaService.ContarVencidas(_idEmpleado).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando datos: " + ex.Message, "AgroSmart", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ====== Menú lateral (placeholders simples; puedes ampliar luego) ======
        private void BtnInicio_Click(object sender, RoutedEventArgs e)
        {
            CargarDatosIniciales();
            MessageBox.Show("Inicio actualizado.", "AgroSmart");
        }

        private void BtnMisTareas_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sección 'Mis tareas'.", "AgroSmart");
        }

        private void BtnProgreso_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sección 'Progreso'.", "AgroSmart");
        }

        private void BtnPerfil_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sección 'Perfil'.", "AgroSmart");
        }

        private void BtnAyuda_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sección 'Ayuda'.", "AgroSmart");
        }

        // ====== Botón 'Registrar avance' (está dentro del ItemsControl con Tag = IdTarea) ======
        private void BtnRegistrarAvance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn == null) return;

                int idTarea;
                if (!int.TryParse(Convert.ToString(btn.Tag), out idTarea))
                {
                    MessageBox.Show("No se pudo identificar la tarea.", "AgroSmart");
                    return;
                }

                // Claude abre con (idTarea, _idEmpleado). Implementamos ese flujo:
                RegistrarAvanceView frm = new RegistrarAvanceView(idTarea, _idEmpleado);
                bool? ok = frm.ShowDialog();
                if (ok == true)
                {
                    // Refrescar tablero
                    CargarDatosIniciales();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el registro de avance: " + ex.Message, "AgroSmart", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
