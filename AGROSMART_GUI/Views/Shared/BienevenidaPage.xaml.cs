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
    /// Lógica de interacción para BienevenidaPage.xaml
    /// </summary>
    public partial class BienvenidaPage : Window
    {
        public BienvenidaPage()
        {
            InitializeComponent();
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            // Abrir ventana de Login
            var loginWindow = new Login();
            loginWindow.ShowDialog();

            // Si el login fue exitoso, cerrar esta ventana
            // Puedes implementar lógica para saber si fue exitoso
        }

        private void BtnRegistro_Click(object sender, RoutedEventArgs e)
        {
            // Abrir ventana de Registro
            var registroWindow = new RegistroPage();
            registroWindow.ShowDialog();
        }
    }
}
