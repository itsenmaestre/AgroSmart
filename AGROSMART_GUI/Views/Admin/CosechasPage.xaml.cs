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
    /// Lógica de interacción para CosechasPage.xaml
    /// </summary>
    public partial class CosechasPage : Page
    {
        private readonly CosechaService _cosechaService = new CosechaService();
        private readonly CultivoService _cultivoService = new CultivoService();
        private readonly int _idAdmin;
        private int? _idCosechaEdicion = null;

        public CosechasPage(int idAdmin)
        {
            InitializeComponent();
            _idAdmin = idAdmin;

            // Establecer fechas por defecto
            dpFechaCosecha.SelectedDate = DateTime.Today;
            dpFechaRegistro.SelectedDate = DateTime.Today;

            CargarCultivos();
            CargarCosechas();
        }

        private void CargarCultivos()
        {
            try
            {
                var cultivos = _cultivoService.Consultar();
                var cultivosVM = cultivos.Select(c => new
                {
                    IdCultivo = c.ID_CULTIVO,
                    Display = $"#{c.ID_CULTIVO} - {c.NOMBRE_LOTE}"
                }).ToList();
                cboCultivo.ItemsSource = cultivosVM;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cultivos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarCosechas()
        {
            try
            {
                var cosechas = _cosechaService.Consultar();
                var viewModels = cosechas.Select(c =>
                {
                    var cultivo = _cultivoService.ObtenerPorId(c.ID_CULTIVO);
                    return new CosechaViewModel
                    {
                        IdCosecha = c.ID_COSECHA,
                        NombreCultivo = cultivo != null ? cultivo.NOMBRE_LOTE : "Cultivo desconocido",
                        FechaCosecha = c.FECHA_COSECHA,
                        CantidadObtenida = c.CANTIDAD_OBTENIDA,
                        UnidadMedida = c.UNIDAD_MEDIDA,
                        Calidad = c.CALIDAD
                    };
                }).ToList();

                dgCosechas.ItemsSource = viewModels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cosechas: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validaciones
                if (cboCultivo.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un cultivo.", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!dpFechaCosecha.SelectedDate.HasValue)
                {
                    MessageBox.Show("Debe seleccionar la fecha de cosecha.", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!dpFechaRegistro.SelectedDate.HasValue)
                {
                    MessageBox.Show("Debe seleccionar la fecha de registro.", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(txtCantidad.Text, out decimal cantidad) || cantidad < 0)
                {
                    MessageBox.Show("La cantidad debe ser un número válido mayor o igual a 0.", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string unidad = cboUnidad.Text?.Trim();
                if (string.IsNullOrWhiteSpace(unidad))
                {
                    MessageBox.Show("Debe especificar la unidad de medida.", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string calidad = (cboCalidad.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (string.IsNullOrEmpty(calidad) && !string.IsNullOrWhiteSpace(cboCalidad.Text))
                {
                    calidad = cboCalidad.Text.Trim();
                }

                if (string.IsNullOrEmpty(calidad))
                {
                    MessageBox.Show("Debe especificar la calidad.", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_idCosechaEdicion.HasValue)
                {
                    // Modo edición
                    var cosecha = _cosechaService.ObtenerPorId(_idCosechaEdicion.Value);
                    if (cosecha != null)
                    {
                        cosecha.ID_CULTIVO = Convert.ToInt32(cboCultivo.SelectedValue);
                        cosecha.FECHA_COSECHA = dpFechaCosecha.SelectedDate.Value;
                        cosecha.FECHA_REGISTRO = dpFechaRegistro.SelectedDate.Value;
                        cosecha.CANTIDAD_OBTENIDA = cantidad;
                        cosecha.UNIDAD_MEDIDA = unidad;
                        cosecha.CALIDAD = calidad;
                        cosecha.OBSERVACIONES = txtObservaciones.Text.Trim();

                        bool resultado = _cosechaService.Actualizar(cosecha);
                        if (resultado)
                        {
                            MessageBox.Show("Cosecha actualizada exitosamente.", "Éxito",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            LimpiarCampos();
                            CargarCosechas();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo actualizar la cosecha.", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    // Modo creación
                    var nuevaCosecha = new COSECHA
                    {
                        ID_CULTIVO = Convert.ToInt32(cboCultivo.SelectedValue),
                        ID_ADMIN_REGISTRO = _idAdmin,
                        FECHA_COSECHA = dpFechaCosecha.SelectedDate.Value,
                        FECHA_REGISTRO = dpFechaRegistro.SelectedDate.Value,
                        CANTIDAD_OBTENIDA = cantidad,
                        UNIDAD_MEDIDA = unidad,
                        CALIDAD = calidad,
                        OBSERVACIONES = txtObservaciones.Text.Trim()
                    };

                    string resultado = _cosechaService.Guardar(nuevaCosecha);

                    if (resultado == "OK")
                    {
                        MessageBox.Show("Cosecha registrada exitosamente.", "Éxito",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        LimpiarCampos();
                        CargarCosechas();
                    }
                    else
                    {
                        MessageBox.Show($"Error al guardar: {resultado}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            _idCosechaEdicion = null;
            cboCultivo.SelectedIndex = -1;
            dpFechaCosecha.SelectedDate = DateTime.Today;
            dpFechaRegistro.SelectedDate = DateTime.Today;
            txtCantidad.Text = "0.00";
            cboUnidad.SelectedIndex = -1;
            cboCalidad.SelectedIndex = 0;
            txtObservaciones.Clear();
            btnGuardar.Content = "💾 Registrar Cosecha";
        }

        private void BtnVer_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.Tag is CosechaViewModel vm)
            {
                string mensaje = $"ID: {vm.IdCosecha}\n" +
                               $"Cultivo: {vm.NombreCultivo}\n" +
                               $"Fecha Cosecha: {vm.FechaCosecha:dd/MM/yyyy}\n" +
                               $"Cantidad: {vm.CantidadObtenida:N2} {vm.UnidadMedida}\n" +
                               $"Calidad: {vm.Calidad}";

                MessageBox.Show(mensaje, "Detalles de la Cosecha",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.Tag is CosechaViewModel vm)
            {
                try
                {
                    var cosecha = _cosechaService.ObtenerPorId(vm.IdCosecha);
                    if (cosecha != null)
                    {
                        _idCosechaEdicion = cosecha.ID_COSECHA;

                        // Cargar datos en el formulario
                        cboCultivo.SelectedValue = cosecha.ID_CULTIVO;
                        dpFechaCosecha.SelectedDate = cosecha.FECHA_COSECHA;
                        dpFechaRegistro.SelectedDate = cosecha.FECHA_REGISTRO;
                        txtCantidad.Text = cosecha.CANTIDAD_OBTENIDA.ToString("0.00");
                        cboUnidad.Text = cosecha.UNIDAD_MEDIDA;

                        // Seleccionar calidad
                        foreach (ComboBoxItem item in cboCalidad.Items)
                        {
                            if (item.Content.ToString() == cosecha.CALIDAD)
                            {
                                cboCalidad.SelectedItem = item;
                                break;
                            }
                        }

                        txtObservaciones.Text = cosecha.OBSERVACIONES;
                        btnGuardar.Content = "💾 Actualizar Cosecha";

                        MessageBox.Show("Datos cargados. Modifique y presione Actualizar.", "Editar",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public class CosechaViewModel
        {
            public int IdCosecha { get; set; }
            public string NombreCultivo { get; set; }
            public DateTime FechaCosecha { get; set; }
            public decimal CantidadObtenida { get; set; }
            public string UnidadMedida { get; set; }
            public string Calidad { get; set; }
        }
    }
}
