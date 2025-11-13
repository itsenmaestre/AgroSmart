using AGROSMART_BLL;
using AGROSMART_ENTITY.ENTIDADES;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly AsignacionTareaService _svc = new AsignacionTareaService();
        private ASIGNACION_TAREA _asignacion;

        
        public RegistrarAvanceView(ASIGNACION_TAREA seleccionada)
        {
            InitializeComponent();
            _asignacion = seleccionada;
            PrecargarUI();
        }

     
        public RegistrarAvanceView(int idTarea, int idEmpleado)
        {
            InitializeComponent();

            var asignaciones = _svc.ListarPorEmpleado(idEmpleado);
            _asignacion = asignaciones.FirstOrDefault(a => a.ID_TAREA == idTarea);

            if (_asignacion == null)
            {
                MessageBox.Show("No se encontró la asignación para esta tarea.", "AgroSmart");
                this.Close();
                return;
            }

            PrecargarUI();
        }

        private void PrecargarUI()
        {
            // Encabezados
            txtInfoTarea.Text = "Tarea código: " + _asignacion.ID_TAREA;
            txtFechaTarea.Text = "Fecha programada: -"; 

          
            txtHorasTrabajadas.Text = _asignacion.HORAS_TRABAJADAS.HasValue
                ? _asignacion.HORAS_TRABAJADAS.Value.ToString("0.##", CultureInfo.InvariantCulture)
                : "0.00";

            txtJornadasTrabajadas.Text = _asignacion.JORNADAS_TRABAJADAS.HasValue
                ? _asignacion.JORNADAS_TRABAJADAS.Value.ToString("0.##", CultureInfo.InvariantCulture)
                : "0.00";

            if (!string.IsNullOrWhiteSpace(_asignacion.ESTADO))
            {
                foreach (object obj in cboEstado.Items)
                {
                    ComboBoxItem item = obj as ComboBoxItem;
                    if (item != null && string.Equals(Convert.ToString(item.Content), _asignacion.ESTADO, StringComparison.OrdinalIgnoreCase))
                    {
                        cboEstado.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal? horas = ParseNullableDecimal(txtHorasTrabajadas.Text);
                decimal? jorn = ParseNullableDecimal(txtJornadasTrabajadas.Text);

                string estado = null;
                ComboBoxItem item = cboEstado.SelectedItem as ComboBoxItem;
                if (item != null) estado = item.Content as string;

                _asignacion.HORAS_TRABAJADAS = horas;
                _asignacion.JORNADAS_TRABAJADAS = jorn;
                _asignacion.ESTADO = estado;

                string rpta = _svc.ActualizarAvance(_asignacion);
                MessageBox.Show(rpta, "AgroSmart", MessageBoxButton.OK, MessageBoxImage.Information);

                if (rpta == "OK")
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Verifica números válidos (usa punto decimal o tu separador).", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "AgroSmart", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private decimal? ParseNullableDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            decimal value;
            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                return value;

            if (decimal.TryParse(input, out value))
                return value;

            throw new FormatException("Número inválido.");
        }
    }
}
