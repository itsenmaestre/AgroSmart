using AGROSMART_BLL;
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
    /// Lógica de interacción para AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        private readonly int _idAdmin;
        private readonly string _nombreAdmin;

        private readonly AdminService _adminService = new AdminService();
        private readonly CultivoService _cultivoService = new CultivoService();
        private readonly TareaService _tareaService = new TareaService();
        private readonly InsumoService _insumoService = new InsumoService();

        // Clases para binding
        private class CultivoProximo
        {
            public int Id { get; set; }
            public string NombreLote { get; set; }
            public string FechaSiembra { get; set; }
            public string FechaCosecha { get; set; }
        }

        private class InsumoAlerta
        {
            public string Nombre { get; set; }
            public string StockActual { get; set; }
            public string StockMinimo { get; set; }
            public string Tipo { get; set; }
        }

        public AdminDashboard(int idAdmin, string nombreCompleto)
        {
            InitializeComponent();

            _idAdmin = idAdmin;
            _nombreAdmin = nombreCompleto;

            if (!string.IsNullOrWhiteSpace(_nombreAdmin))
                txtBienvenida.Text = $"Hola, {_nombreAdmin}";

            CargarDashboard();
        }

        private void CargarDashboard()
        {
            try
            {
                // Estadísticas generales
                var stats = _adminService.ObtenerEstadisticas(_idAdmin);

                txtCultivosActivos.Text = stats.ContainsKey("CultivosActivos")
                    ? stats["CultivosActivos"].ToString() : "0";

                txtTareasPendientes.Text = stats.ContainsKey("TareasPendientes")
                    ? stats["TareasPendientes"].ToString() : "0";

                txtTotalEmpleados.Text = stats.ContainsKey("TotalEmpleados")
                    ? stats["TotalEmpleados"].ToString() : "0";

                // Alertas de stock
                int alertasStock = _insumoService.ContarInsumosConStockBajo();
                txtAlertasStock.Text = alertasStock.ToString();

                // Cultivos próximos a cosechar (30 días)
                var cultivosProximos = _cultivoService.ObtenerCultivosProximosACosechar(30);
                var cultivosDTO = cultivosProximos.Select(c => new CultivoProximo
                {
                    Id = c.ID_CULTIVO,
                    NombreLote = c.NOMBRE_LOTE,
                    FechaSiembra = c.FECHA_SIEMBRA.ToString("dd/MM/yyyy"),
                    FechaCosecha = c.FECHA_COSECHA_ESTIMADA.ToString("dd/MM/yyyy")
                }).ToList();

                lstCultivosProximos.ItemsSource = cultivosDTO;

                // Insumos con stock bajo
                var insumosAlerta = _insumoService.ObtenerInsumosConStockBajo();
                var insumosDTO = insumosAlerta.Select(i => new InsumoAlerta
                {
                    Nombre = i.NOMBRE,
                    StockActual = i.STOCK_ACTUAL.ToString("0.##"),
                    StockMinimo = i.STOCK_MINIMO.ToString("0.##"),
                    Tipo = i.TIPO
                }).ToList();

                lstInsumosAlerta.ItemsSource = insumosDTO;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando dashboard: {ex.Message}",
                    "AgroSmart", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ===== EVENTOS DE MENÚ =====

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            CargarDashboard();
        }

        private void BtnCultivos_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Abrir ventana de gestión de cultivos
            MessageBox.Show("Módulo de Cultivos en desarrollo", "AgroSmart");
        }

        private void BtnTareas_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Abrir ventana de gestión de tareas
            MessageBox.Show("Módulo de Tareas en desarrollo", "AgroSmart");
        }

        private void BtnInsumos_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Abrir ventana de gestión de insumos
            MessageBox.Show("Módulo de Insumos en desarrollo", "AgroSmart");
        }

        private void BtnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Abrir ventana de gestión de empleados
            MessageBox.Show("Módulo de Empleados en desarrollo", "AgroSmart");
        }

        private void BtnCosechas_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Abrir ventana de registro de cosechas
            MessageBox.Show("Módulo de Cosechas en desarrollo", "AgroSmart");
        }

        private void BtnReportes_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Abrir ventana de reportes
            MessageBox.Show("Módulo de Reportes en desarrollo", "AgroSmart");
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar sesión?",
                "Cerrar Sesión", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var bienvenida = new Shared.BienvenidaPage();
                bienvenida.Show();
                this.Close();
            }
        }
    }
}
