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
    public partial class AdmminView : Window
    {
        private readonly int _idAdmin;
        private readonly string _nombreAdmin;

        public AdmminView(int idAdmin, string nombreCompleto)
        {
            InitializeComponent();

            _idAdmin = idAdmin;
            _nombreAdmin = nombreCompleto;

            // Actualizar nombre en UI
            if (!string.IsNullOrWhiteSpace(_nombreAdmin))
                txtUserName.Text = _nombreAdmin;

            // Seleccionar primer item por defecto
            MenuListBox.SelectedIndex = 0;
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuListBox.SelectedItem is ListBoxItem item)
            {
                string tag = item.Tag.ToString();

                switch (tag)
                {
                    case "📦":
                        AdminFrame.Navigate(new SuministrosPage(_idAdmin));
                        break;
                    case "✅":
                        AdminFrame.Navigate(new CrearTareasPage(_idAdmin));
                        break;
                    case "👥":
                        AdminFrame.Navigate(new AsignarTareaPage(_idAdmin));
                        break;
                    case "🌾":
                        AdminFrame.Navigate(new CultivosPage(_idAdmin));
                        break;
                    case "🌽":
                        AdminFrame.Navigate(new CosechasPage(_idAdmin));
                        break;
                }
            }
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
