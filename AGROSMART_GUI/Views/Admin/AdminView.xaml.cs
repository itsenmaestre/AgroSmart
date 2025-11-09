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

            if (!string.IsNullOrWhiteSpace(_nombreAdmin))
                txtUserName.Text = _nombreAdmin;

            // Cargar página de inicio por defecto
            CargarPaginaInicio();
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuListBox.SelectedItem is ListBoxItem item)
            {
                string tag = item.Tag?.ToString();

                switch (tag)
                {
                    case "📦":
                        CargarPaginaInsumos();
                        break;
                    case "✅":
                        CargarPaginaCrearTareas();
                        break;
                    case "👥":
                        CargarPaginaAsignarTarea();
                        break;
                    case "🌾":
                        CargarPaginaCultivos();
                        break;
                    case "🌽":
                        CargarPaginaCosechas();
                        break;
                }
            }
        }

        private void CargarPaginaInicio()
        {
            // Página temporal de bienvenida
            var page = new Page();
            var stack = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var welcome = new TextBlock
            {
                Text = $"Bienvenido, {_nombreAdmin}",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 20)
            };

            var info = new TextBlock
            {
                Text = "Selecciona una opción del menú lateral",
                FontSize = 16
            };

            stack.Children.Add(welcome);
            stack.Children.Add(info);
            page.Content = stack;

            AdminFrame.Navigate(page);
        }

        private void CargarPaginaInsumos()
        {
            MostrarPaginaTemporal("Gestión de Insumos",
                "Aquí podrás administrar el inventario de insumos");
        }

        private void CargarPaginaCrearTareas()
        {
            MostrarPaginaTemporal("Crear Tareas",
                "Aquí podrás crear nuevas tareas para los cultivos");
        }

        private void CargarPaginaAsignarTarea()
        {
            MostrarPaginaTemporal("Asignar Tareas",
                "Aquí podrás asignar tareas a los empleados");
        }

        private void CargarPaginaCultivos()
        {
            MostrarPaginaTemporal("Gestión de Cultivos",
                "Aquí podrás administrar los cultivos");
        }

        private void CargarPaginaCosechas()
        {
            MostrarPaginaTemporal("Registro de Cosechas",
                "Aquí podrás registrar las cosechas realizadas");
        }

        private void MostrarPaginaTemporal(string titulo, string descripcion)
        {
            var page = new Page();
            var stack = new StackPanel
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
                Margin = new Thickness(0, 0, 0, 15)
            };

            var desc = new TextBlock
            {
                Text = descripcion,
                FontSize = 16,
                Foreground = System.Windows.Media.Brushes.Gray
            };

            var devNote = new TextBlock
            {
                Text = "📝 Funcionalidad en desarrollo",
                FontSize = 14,
                Foreground = System.Windows.Media.Brushes.Orange,
                Margin = new Thickness(0, 30, 0, 0)
            };

            stack.Children.Add(title);
            stack.Children.Add(desc);
            stack.Children.Add(devNote);
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
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
