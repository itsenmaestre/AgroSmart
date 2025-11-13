using AGROSMART_BLL;
using AGROSMART_ENTITY.ENTIDADES_DTOS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AGROSMART_GUI.Views.Admin
{
    /// <summary>
    /// Lógica de interacción para GastosPage.xaml
    /// </summary>
    public partial class GastosPage : Page
    {
        private readonly GastoTareaService _gastoService = new GastoTareaService();

        public GastosPage(int idAdmin)
        {
            InitializeComponent();
            CargarGastos();
        }

        private void CargarGastos()
        {
            try
            {
                var datos = _gastoService.ObtenerGastosTareasFinalizadas();
                var viewModels = datos.Select(MapearViewModel).ToList();

                lstTareas.ItemsSource = viewModels;
                bool hayDatos = viewModels.Count > 0;
                pnlVacio.Visibility = hayDatos ? Visibility.Collapsed : Visibility.Visible;
                lstTareas.Visibility = hayDatos ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los gastos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private GastoTareaViewModel MapearViewModel(GastoTareaDTO dto)
        {
            return new GastoTareaViewModel
            {
                IdTarea = dto.IdTarea,
                NombreTarea = $"#{dto.IdTarea} - {dto.TipoActividad}",
                Cultivo = dto.NombreCultivo,
                FechaProgramada = dto.FechaProgramada,
                Estado = string.IsNullOrWhiteSpace(dto.Estado) ? "COMPLETADA" : dto.Estado,
                TotalInsumos = dto.TotalInsumos,
                TotalManoObra = dto.TotalManoObra,
                CostoTransporte = dto.CostoTransporte,
                TotalGasto = dto.TotalGasto,
                Insumos = dto.DetallesInsumos.Select(i => new GastoInsumoViewModel
                {
                    Nombre = i.NombreInsumo,
                    Cantidad = i.CantidadUsada,
                    CostoUnitario = i.CostoUnitario,
                    CostoTotal = i.CostoTotal
                }).ToList(),
                Empleados = dto.DetallesEmpleados.Select(e => new GastoEmpleadoViewModel
                {
                    Nombre = e.NombreEmpleado,
                    Jornadas = e.JornadasTrabajadas,
                    Horas = e.HorasTrabajadas,
                    Pago = e.MontoCalculado,
                    PagoAcordado = e.PagoAcordado
                }).ToList()
            };
        }

        private void BtnAgregarInsumo_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.Tag is GastoTareaViewModel vm)
            {
                var ventana = new RegistrarInsumoTareaWindow(vm.IdTarea);
                bool? resultado = ventana.ShowDialog();
                if (resultado == true)
                {
                    MessageBox.Show("Insumo registrado correctamente y stock actualizado.", "Gastos",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    CargarGastos();
                }
            }
        }

        public class GastoTareaViewModel
        {
            public int IdTarea { get; set; }
            public string NombreTarea { get; set; }
            public string Cultivo { get; set; }
            public DateTime FechaProgramada { get; set; }
            public string Estado { get; set; }
            public decimal TotalInsumos { get; set; }
            public decimal TotalManoObra { get; set; }
            public decimal CostoTransporte { get; set; }
            public decimal TotalGasto { get; set; }
            public List<GastoInsumoViewModel> Insumos { get; set; } = new List<GastoInsumoViewModel>();
            public List<GastoEmpleadoViewModel> Empleados { get; set; } = new List<GastoEmpleadoViewModel>();
        }

        public class GastoInsumoViewModel
        {
            public string Nombre { get; set; }
            public decimal Cantidad { get; set; }
            public decimal CostoUnitario { get; set; }
            public decimal CostoTotal { get; set; }
        }

        public class GastoEmpleadoViewModel
        {
            public string Nombre { get; set; }
            public decimal Jornadas { get; set; }
            public decimal Horas { get; set; }
            public decimal Pago { get; set; }
            public decimal? PagoAcordado { get; set; }
        }
    }

    public class NullOrZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Visible;

            if (value is int intValue)
                return intValue == 0 ? Visibility.Visible : Visibility.Collapsed;

            if (value is double doubleValue)
                return Math.Abs(doubleValue) < double.Epsilon ? Visibility.Visible : Visibility.Collapsed;

            if (value is decimal decimalValue)
                return decimalValue == 0 ? Visibility.Visible : Visibility.Collapsed;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
