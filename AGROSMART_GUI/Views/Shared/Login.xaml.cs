using System;
using System.Windows;                 
using System.Windows.Controls;         
using AGROSMART_BLL;                   
using AGROSMART_ENTITY.ENTIDADES;

namespace AGROSMART_GUI.Views.Shared
{
    public partial class Login : Window
    {
        private readonly UsuarioService _service = new UsuarioService();

        public Login()
        {
            InitializeComponent();
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar que se ingrese un ID numérico
                if (!int.TryParse(txtIdUsuario.Text, out int id))
                    throw new Exception("El ID de usuario (cédula) debe ser numérico.");

                // Validar contraseña
                if (string.IsNullOrWhiteSpace(txtPass.Password))
                    throw new Exception("La contraseña es obligatoria.");

                // Autenticar usuario
                USUARIO u = _service.Login(id, txtPass.Password);

                if (u == null)
                    throw new Exception("Credenciales incorrectas.");

                // Verificar rol y redirigir
                if (_service.EsAdministrador(id))
                {
                    // Abrir Dashboard de Administrador
                    MessageBox.Show(
                        $"Bienvenido Administrador: {u.PRIMER_NOMBRE} {u.PRIMER_APELLIDO}",
                        "AgroSmart - Admin",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    // TODO: Crear y abrir AdminDashboard
                    // var adminWindow = new Views.Admin.AdminDashboard(u.ID_USUARIO, $"{u.PRIMER_NOMBRE} {u.PRIMER_APELLIDO}");
                    // adminWindow.Show();
                    // this.Close();
                }
                else if (_service.EsEmpleado(id))
                {
                    // Abrir Dashboard de Empleado
                    string nombreCompleto = $"{u.PRIMER_NOMBRE} {u.PRIMER_APELLIDO}";

                    var empleadoWindow = new Views.Empleado.EmpleadoView(u.ID_USUARIO, nombreCompleto);
                    empleadoWindow.Show();
                    this.Close();
                }
                else
                {
                    throw new Exception("El usuario no tiene un rol asignado. Contacte al administrador.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error de Inicio de Sesión",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        // Evento para abrir ventana de registro
        private void BtnRegistro_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var registroWindow = new RegistroPage();
            registroWindow.ShowDialog();
        }
    }
}
