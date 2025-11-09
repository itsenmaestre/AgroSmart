using AGROSMART_BLL;
using AGROSMART_ENTITY.ENTIDADES;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AGROSMART_GUI.Views.Empleado
{
    /// <summary>
    /// Lógica de interacción para EmpleadoView.xaml
    /// </summary>
    public partial class EmpleadoView : Window
    {
        private readonly int _idEmpleado;
        private readonly string _nombreEmpleado;

        public EmpleadoView(int idEmpleadoActual, string nombreCompleto = null)
        {
            InitializeComponent();

            _idEmpleado = idEmpleadoActual;
            _nombreEmpleado = nombreCompleto;

            // Cargar página de inicio por defecto
            CargarPaginaInicio();
        }

        private void BtnInicio_Click(object sender, RoutedEventArgs e)
        {
            ResetearBotones();
            btnInicio.Background = new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#5CB85C"));
            CargarPaginaInicio();
        }

        private void BtnMisTareas_Click(object sender, RoutedEventArgs e)
        {
            ResetearBotones();
            btnMisTareas.Background = new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#5CB85C"));
            EmpleadoFrame.Navigate(new MisTareasPage(_idEmpleado));
        }

        private void BtnProgreso_Click(object sender, RoutedEventArgs e)
        {
            ResetearBotones();
            btnProgreso.Background = new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#5CB85C"));
            EmpleadoFrame.Navigate(new ProgresoPage(_idEmpleado));
        }

        private void BtnPerfil_Click(object sender, RoutedEventArgs e)
        {
            ResetearBotones();
            btnPerfil.Background = new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#5CB85C"));
            EmpleadoFrame.Navigate(new PerfilPage(_idEmpleado));
        }

        private void BtnAyuda_Click(object sender, RoutedEventArgs e)
        {
            ResetearBotones();
            btnAyuda.Background = new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#5CB85C"));
            EmpleadoFrame.Navigate(new AyudaPage());
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

        private void ResetearBotones()
        {
            btnInicio.Background = System.Windows.Media.Brushes.Transparent;
            btnMisTareas.Background = System.Windows.Media.Brushes.Transparent;
            btnProgreso.Background = System.Windows.Media.Brushes.Transparent;
            btnPerfil.Background = System.Windows.Media.Brushes.Transparent;
            btnAyuda.Background = System.Windows.Media.Brushes.Transparent;
        }

        private void CargarPaginaInicio()
        {
            EmpleadoFrame.Navigate(new InicioEmpleadoPage(_idEmpleado, _nombreEmpleado));
        }
    }
}
