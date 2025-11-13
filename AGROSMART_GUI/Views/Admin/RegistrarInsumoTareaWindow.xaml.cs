using AGROSMART_BLL;
using AGROSMART_ENTITY.ENTIDADES;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AGROSMART_GUI.Views.Admin
{
    /// <summary>
    /// Lógica de interacción para RegistrarInsumoTareaWindow.xaml
    /// </summary>
    public partial class RegistrarInsumoTareaWindow : Window
    {
        private readonly int _idTarea;
        private readonly InsumoService _insumoService = new InsumoService();
        private readonly DetalleTareaService _detalleService = new DetalleTareaService();
        private List<InsumoOption> _insumosDisponibles = new List<InsumoOption>();

        public RegistrarInsumoTareaWindow(int idTarea)
        {
            InitializeComponent();
            _idTarea = idTarea;
            CargarInsumos();
        }

        private void CargarInsumos()
        {
            try
            {
                var insumos = _insumoService.Consultar();
                _insumosDisponibles = insumos
                    .Where(i => string.Equals(i.TIPO, "CONSUMIBLE", StringComparison.OrdinalIgnoreCase))
                    .Select(i => new InsumoOption
                    {
                        Id = i.ID_INSUMO,
                        Nombre = $"{i.NOMBRE} (Stock: {i.STOCK_ACTUAL:N2})",
                        CostoUnitario = i.COSTO_UNITARIO,
                        StockDisponible = i.STOCK_ACTUAL
                    })
                    .OrderBy(i => i.Nombre)
                    .ToList();

                cboInsumo.ItemsSource = _insumosDisponibles;
                if (_insumosDisponibles.Count > 0)
                    cboInsumo.SelectedIndex = 0;
                else
                    MessageBox.Show("No hay insumos consumibles disponibles.", "Insumos",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los insumos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActualizarResumen()
        {
            if (cboInsumo.SelectedItem is not InsumoOption opcion)
            {
                txtStockDisponible.Text = string.Empty;
                txtCostoUnidad.Text = string.Empty;
                txtCostoTotal.Text = string.Empty;
                return;
            }

            txtStockDisponible.Text = opcion.StockDisponible.ToString("N2");
            txtCostoUnidad.Text = opcion.CostoUnitario.ToString("C2", CultureInfo.CurrentCulture);

            if (decimal.TryParse(txtCantidad.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal cantidad) ||
                decimal.TryParse(txtCantidad.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out cantidad))
            {
                if (cantidad < 0)
                {
                    txtCostoTotal.Text = string.Empty;
                    return;
                }

                decimal total = opcion.CostoUnitario * cantidad;
                txtCostoTotal.Text = total.ToString("C2", CultureInfo.CurrentCulture);
            }
            else
            {
                txtCostoTotal.Text = string.Empty;
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (cboInsumo.SelectedItem is not InsumoOption opcion)
            {
                MessageBox.Show("Selecciona un insumo válido.", "Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtCantidad.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal cantidad) &&
                !decimal.TryParse(txtCantidad.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out cantidad))
            {
                MessageBox.Show("Ingresa una cantidad numérica válida.", "Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero.", "Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var detalle = new DETALLE_TAREA
                {
                    ID_TAREA = _idTarea,
                    ID_INSUMO = opcion.Id,
                    CANTIDAD_USADA = cantidad
                };

                string resultado = _detalleService.RegistrarInsumosConDescuento(_idTarea, new List<DETALLE_TAREA> { detalle });
                if (resultado == "OK")
                {
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show(resultado, "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo registrar el insumo: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CboInsumo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarResumen();
        }

        private void TxtCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualizarResumen();
        }

        private class InsumoOption
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public decimal CostoUnitario { get; set; }
            public decimal StockDisponible { get; set; }
        }
    }
}
