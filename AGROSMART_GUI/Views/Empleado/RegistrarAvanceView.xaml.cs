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

namespace AGROSMART_GUI.Views.Empleado
{
    /// <summary>
    /// Lógica de interacción para RegistrarAvanceView.xaml
    /// </summary>
    public partial class RegistrarAvanceView : Window
    {
        private readonly int _idTarea;
        private readonly int _idEmpleado;

        // TODO: Crear AsignacionTareaService en BLL
        // private readonly AsignacionTareaService _service = new AsignacionTareaService();

        public RegistrarAvanceView(int idTarea, int idEmpleado)
        {
            InitializeComponent();
            _idTarea = idTarea;
            _idEmpleado = idEmpleado;

            CargarInfoTarea();
        }

        private void CargarInfoTarea()
        {
            // TODO: Obtener información de la tarea desde la base de datos
            // Ejemplo:
            // var tarea = _tareaService.ObtenerPorId(_idTarea);
            // txtInfoTarea.Text = $"Tarea: {tarea.TIPO_ACTIVIDAD} - #{tarea.ID_TAREA}";
            // txtFechaTarea.Text = $"Fecha programada: {tarea.FECHA_PROGRAMADA:dd 'de' MMMM, yyyy}";

            txtInfoTarea.Text = $"Tarea #{_idTarea}";
            txtFechaTarea.Text = $"Empleado ID: {_idEmpleado}";
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validaciones básicas
                if (!decimal.TryParse(txtHorasTrabajadas.Text, out decimal horas) || horas < 0)
                {
                    MessageBox.Show("Las horas trabajadas deben ser un número positivo.",
                        "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(txtJornadasTrabajadas.Text, out decimal jornadas) || jornadas < 0)
                {
                    MessageBox.Show("Las jornadas trabajadas deben ser un número positivo.",
                        "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string estado = ((System.Windows.Controls.ComboBoxItem)cboEstado.SelectedItem)?.Content?.ToString() ?? "PENDIENTE";

                // TODO: Crear entidad y guardar en BD
                // Ejemplo:
                /*
                var asignacion = new ASIGNACION_TAREA
                {
                    ID_TAREA = _idTarea,
                    ID_EMPLEADO = _idEmpleado,
                    HORAS_TRABAJADAS = horas,
                    JORNADAS_TRABAJADAS = jornadas,
                    ESTADO = estado,
                    FECHA_ASIGNACION = DateTime.Now
                };

                var service = new AsignacionTareaService();
                string resultado = service.ActualizarAvance(asignacion);

                if (resultado == "OK")
                {
                    MessageBox.Show("Avance registrado correctamente.", "Éxito", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show($"Error al registrar avance: {resultado}", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                */

                // Simulación temporal
                MessageBox.Show($"Avance registrado:\n\n" +
                    $"Tarea: {_idTarea}\n" +
                    $"Horas: {horas}\n" +
                    $"Jornadas: {jornadas}\n" +
                    $"Estado: {estado}\n" +
                    $"Observaciones: {txtObservaciones.Text}",
                    "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar avance: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
