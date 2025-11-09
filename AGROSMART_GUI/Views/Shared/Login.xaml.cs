using AGROSMART_BLL;
using AGROSMART_ENTITY.ENTIDADES;
using AGROSMART_GUI.Views.Empleado;
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

namespace AGROSMART_GUI.Views.Shared
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly UsuarioService _usuarioService = new UsuarioService();

        public Login()
        {
            InitializeComponent();
            this.Loaded += (s, e) => txbId.Focus();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar ID numérico
                if (!int.TryParse(txbId.Text, out int id))
                {
                    MessageBox.Show("El ID debe ser numérico.", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string contrasena = txbContra.Password;

                if (string.IsNullOrWhiteSpace(contrasena))
                {
                    MessageBox.Show("La contraseña es obligatoria.", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Autenticar usuario
                USUARIO usuario = _usuarioService.Login(id, contrasena);

                // Verificar rol
                if (_usuarioService.EsAdministrador(id))
                {
                    string nombreCompleto = $"{usuario.PRIMER_NOMBRE} {usuario.PRIMER_APELLIDO}";

                    var adminWindow = new AdmminView(id, nombreCompleto);
                    adminWindow.Show();

                    // Cerrar MainWindow
                    Window.GetWindow(this)?.Close();
                }
                else if (_usuarioService.EsEmpleado(id))
                {
                    string nombreCompleto = $"{usuario.PRIMER_NOMBRE} {usuario.PRIMER_APELLIDO}";

                    var empleadoWindow = new EmpleadoView(id, nombreCompleto);
                    empleadoWindow.Show();

                    // Cerrar MainWindow
                    Window.GetWindow(this)?.Close();
                }
                else
                {
                    MessageBox.Show("Usuario sin rol asignado. Contacte al administrador.",
                        "Error de Acceso", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuPage());
        }

        // ... Resto de eventos (GotFocus, LostFocus, KeyDown)
    }
}
