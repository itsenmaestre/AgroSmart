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
    /// Lógica de interacción para InicioAdminPage.xaml
    /// </summary>
    public partial class InicioAdminPage : Page
    {
        private readonly AdminService _adminService = new AdminService();
        private readonly int _idAdmin;
        private readonly string _nombreAdmin;

        public InicioAdminPage(int idAdmin, string nombreAdmin)
        {
            InitializeComponent();
            _idAdmin = idAdmin;
            _nombreAdmin = nombreAdmin;

            if (!string.IsNullOrWhiteSpace(_nombreAdmin))
                txtBienvenida.Text = $"Bienvenido, {_nombreAdmin}";

            // Mostrar fecha actual
            txtFechaHoy.Text = DateTime.Now.ToString("dddd, dd 'de' MMMM",
                new System.Globalization.CultureInfo("es-ES"));

            CargarEstadisticas();
        }

        private void CargarEstadisticas()
        {
            try
            {
                var stats = _adminService.ObtenerEstadisticas(_idAdmin);

                // Método auxiliar para obtener valores de forma segura
                int GetStatValue(string key)
                {
                    return stats.ContainsKey(key) ? stats[key] : 0;
                }

                // Asignar valores a los controles
                txtCultivosActivos.Text = GetStatValue("CultivosActivos").ToString();
                txtTareasCreadas.Text = GetStatValue("TareasCreadas").ToString();
                txtTotalEmpleados.Text = GetStatValue("TotalEmpleados").ToString();
                txtTareasPendientes.Text = GetStatValue("TareasPendientes").ToString();

                // Generar resumen
                int cultivos = GetStatValue("CultivosActivos");
                int tareas = GetStatValue("TareasCreadas");
                int empleados = GetStatValue("TotalEmpleados");
                int pendientes = GetStatValue("TareasPendientes");

                txtResumen.Text = $"• Estás supervisando {cultivos} cultivos activos.\n" +
                                 $"• Has creado {tareas} tareas en total.\n" +
                                 $"• Tienes {empleados} empleados registrados.\n" +
                                 $"• Hay {pendientes} tareas pendientes de asignación.";
            }
            catch (Exception ex)
            {
                txtResumen.Text = $"Error al cargar estadísticas: {ex.Message}";

                // Valores por defecto en caso de error
                txtCultivosActivos.Text = "0";
                txtTareasCreadas.Text = "0";
                txtTotalEmpleados.Text = "0";
                txtTareasPendientes.Text = "0";
            }
        }

        private void BtnCrearTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Navegar a CrearTareasPage
                var parentWindow = Window.GetWindow(this) as AdminView;
                if (parentWindow != null)
                {
                    var frame = parentWindow.FindName("AdminFrame") as Frame;
                    if (frame != null)
                    {
                        frame.Navigate(new CrearTareasPage(_idAdmin));
                    }
                    else
                    {
                        MessageBox.Show("No se pudo acceder al Frame de navegación.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al navegar: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRegistrarCultivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Navegar a CultivosPage
                var parentWindow = Window.GetWindow(this) as AdminView;
                if (parentWindow != null)
                {
                    var frame = parentWindow.FindName("AdminFrame") as Frame;
                    if (frame != null)
                    {
                        frame.Navigate(new CultivosPage(_idAdmin));
                    }
                    else
                    {
                        MessageBox.Show("No se pudo acceder al Frame de navegación.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al navegar: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGestionarInsumos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Navegar a InsumosPage
                var parentWindow = Window.GetWindow(this) as AdminView;
                if (parentWindow != null)
                {
                    var frame = parentWindow.FindName("AdminFrame") as Frame;
                    if (frame != null)
                    {
                        frame.Navigate(new InsumosPage(_idAdmin));
                    }
                    else
                    {
                        MessageBox.Show("No se pudo acceder al Frame de navegación.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al navegar: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
