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

                txtCultivosActivos.Text = stats.ContainsKey("CultivosActivos")
                    ? stats["CultivosActivos"].ToString() : "0";

                txtTareasCreadas.Text = stats.ContainsKey("TareasCreadas")
                    ? stats["TareasCreadas"].ToString() : "0";

                txtTotalEmpleados.Text = stats.ContainsKey("TotalEmpleados")
                    ? stats["TotalEmpleados"].ToString() : "0";

                txtTareasPendientes.Text = stats.ContainsKey("TareasPendientes")
                    ? stats["TareasPendientes"].ToString() : "0";

                // Resumen
                txtResumen.Text = $"• Estás supervisando {stats.GetValueOrDefault("CultivosActivos", 0)} cultivos activos.\n" +
                                 $"• Has creado {stats.GetValueOrDefault("TareasCreadas", 0)} tareas en total.\n" +
                                 $"• Tienes {stats.GetValueOrDefault("TotalEmpleados", 0)} empleados registrados.\n" +
                                 $"• Hay {stats.GetValueOrDefault("TareasPendientes", 0)} tareas pendientes de asignación.";
            }
            catch (Exception ex)
            {
                txtResumen.Text = $"Error al cargar estadísticas: {ex.Message}";
            }
        }

        private void BtnCrearTarea_Click(object sender, RoutedEventArgs e)
        {
            // Navegar a CrearTareasPage
            var parentWindow = Window.GetWindow(this) as AdminView;
            if (parentWindow != null)
            {
                var frame = parentWindow.FindName("AdminFrame") as Frame;
                frame?.Navigate(new CrearTareasPage(_idAdmin));
            }
        }

        private void BtnRegistrarCultivo_Click(object sender, RoutedEventArgs e)
        {
            // Navegar a CultivosPage
            var parentWindow = Window.GetWindow(this) as AdminView;
            if (parentWindow != null)
            {
                var frame = parentWindow.FindName("AdminFrame") as Frame;
                frame?.Navigate(new CultivosPage(_idAdmin));
            }
        }

        private void BtnGestionarInsumos_Click(object sender, RoutedEventArgs e)
        {
            // Navegar a InsumosPage
            var parentWindow = Window.GetWindow(this) as AdminView;
            if (parentWindow != null)
            {
                var frame = parentWindow.FindName("AdminFrame") as Frame;
                frame?.Navigate(new InsumosPage(_idAdmin));
            }
        }
    }
}
