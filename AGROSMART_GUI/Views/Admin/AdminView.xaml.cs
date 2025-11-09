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
    /// Lógica de interacción para AdminView.xaml
    /// </summary>
    public partial class AdminView : Window
    {
        private readonly int _idAdmin;
        private readonly string _nombreAdmin;

        public AdminView(int idAdmin, string nombreCompleto)
        {
            InitializeComponent();

            _idAdmin = idAdmin;
            _nombreAdmin = nombreCompleto;

            // Mostrar nombre del admin
            if (!string.IsNullOrWhiteSpace(_nombreAdmin))
                txtUserName.Text = _nombreAdmin;

            // Cargar página de inicio por defecto
            MenuListBox.SelectedIndex = 0;
            CargarPaginaInicio();
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuListBox.SelectedItem is ListBoxItem item)
            {
                string tag = item.Tag?.ToString();

                switch (tag)
                {
                    case "📦": // Insumos
                        CargarPaginaInsumos();
                        break;
                    case "✅": // Crear Tareas
                        CargarPaginaCrearTareas();
                        break;
                    case "👥": // Asignar Tarea
                        CargarPaginaAsignarTarea();
                        break;
                    case "🌾": // Cultivos
                        CargarPaginaCultivos();
                        break;
                    case "🌽": // Cosechas
                        CargarPaginaCosechas();
                        break;
                    default:
                        CargarPaginaInicio();
                        break;
                }
            }
        }

        private void CargarPaginaInicio()
        {
            try
            {
                // Puedes crear una página de dashboard con estadísticas
                var page = new InicioAdminPage(_idAdmin, _nombreAdmin);
                AdminFrame.Navigate(page);
            }
            catch
            {
                // Si no existe InicioAdminPage, mostrar página temporal
                MostrarPaginaTemporal("Dashboard",
                    $"Bienvenido {_nombreAdmin}.\nSelecciona una opción del menú lateral.");
            }
        }

        private void CargarPaginaInsumos()
        {
            try
            {
                var page = new InsumosPage(_idAdmin);
                AdminFrame.Navigate(page);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar Insumos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarPaginaCrearTareas()
        {
            try
            {
                var page = new CrearTareasPage(_idAdmin);
                AdminFrame.Navigate(page);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar Crear Tareas: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarPaginaAsignarTarea()
        {
            try
            {
                var page = new AsignarEmpleadosPage(_idAdmin);
                AdminFrame.Navigate(page);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar Asignar Tareas: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarPaginaCultivos()
        {
            try
            {
                var page = new CultivosPage(_idAdmin);
                AdminFrame.Navigate(page);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar Cultivos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarPaginaCosechas()
        {
            try
            {
                var page = new CosechasPage(_idAdmin);
                AdminFrame.Navigate(page);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar Cosechas: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MostrarPaginaTemporal(string titulo, string descripcion)
        {
            var page = new Page();
            var stack = new System.Windows.Controls.StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(40)
            };

            var title = new TextBlock
            {
                Text = titulo,
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkGreen,
                Margin = new Thickness(0, 0, 0, 15),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var desc = new TextBlock
            {
                Text = descripcion,
                FontSize = 16,
                Foreground = System.Windows.Media.Brushes.Gray,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                MaxWidth = 400
            };

            stack.Children.Add(title);
            stack.Children.Add(desc);
            page.Content = stack;

            AdminFrame.Navigate(page);
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "¿Está seguro que desea cerrar sesión?",
                "Cerrar Sesión",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Volver a MainWindow (login)
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
