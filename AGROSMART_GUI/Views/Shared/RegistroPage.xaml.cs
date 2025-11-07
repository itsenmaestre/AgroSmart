using AGROSMART_BLL;
using AGROSMART_ENTITY.ENTIDADES;
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
    /// Lógica de interacción para RegistroPage.xaml
    /// </summary>
    public partial class RegistroPage : Window
    {
        private readonly UsuarioService _service = new UsuarioService();

        public RegistroPage()
        {
            InitializeComponent();
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(txtCedula.Text))
                    throw new Exception("La cédula es obligatoria.");

                if (!int.TryParse(txtCedula.Text, out int cedula))
                    throw new Exception("La cédula debe ser un número válido.");

                if (string.IsNullOrWhiteSpace(txtPrimerNombre.Text))
                    throw new Exception("El primer nombre es obligatorio.");

                if (string.IsNullOrWhiteSpace(txtPrimerApellido.Text))
                    throw new Exception("El primer apellido es obligatorio.");

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                    throw new Exception("El correo electrónico es obligatorio.");

                if (string.IsNullOrWhiteSpace(txtTelefono.Text))
                    throw new Exception("El teléfono es obligatorio.");

                if (string.IsNullOrWhiteSpace(txtContrasena.Password))
                    throw new Exception("La contraseña es obligatoria.");

                if (txtContrasena.Password != txtConfirmarContrasena.Password)
                    throw new Exception("Las contraseñas no coinciden.");

                // Crear objeto USUARIO
                var nuevoUsuario = new USUARIO
                {
                    ID_USUARIO = cedula,
                    PRIMER_NOMBRE = txtPrimerNombre.Text.Trim(),
                    SEGUNDO_NOMBRE = string.IsNullOrWhiteSpace(txtSegundoNombre.Text) ? null : txtSegundoNombre.Text.Trim(),
                    PRIMER_APELLIDO = txtPrimerApellido.Text.Trim(),
                    SEGUNDO_APELLIDO = string.IsNullOrWhiteSpace(txtSegundoApellido.Text) ? null : txtSegundoApellido.Text.Trim(),
                    CEDULA = txtCedula.Text.Trim(),
                    EMAIL = txtEmail.Text.Trim(),
                    CONTRASENA = txtContrasena.Password,
                    TELEFONO = txtTelefono.Text.Trim()
                };

                // Crear objeto EMPLEADO (valores por defecto para monto por hora y jornal)
                var nuevoEmpleado = new EMPLEADO
                {
                    ID_USUARIO = cedula,
                    MONTO_POR_HORA = 0,     // El admin lo definirá después
                    MONTO_POR_JORNAL = 0    // El admin lo definirá después
                };

                // Registrar empleado (transacción en BLL/DAL)
                string resultado = _service.RegistrarEmpleado(nuevoUsuario, nuevoEmpleado);

                if (resultado == "OK")
                {
                    MessageBox.Show(
                        $"¡Registro exitoso!\n\n" +
                        $"Usuario: {nuevoUsuario.PRIMER_NOMBRE} {nuevoUsuario.PRIMER_APELLIDO}\n" +
                        $"Cédula: {nuevoUsuario.ID_USUARIO}\n\n" +
                        $"Ya puedes iniciar sesión como Empleado.",
                        "Registro Completado",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    this.DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show($"Error al registrar: {resultado}",
                        "Error de Registro",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error de Validación",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
