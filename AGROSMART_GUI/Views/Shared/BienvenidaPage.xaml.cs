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
using System.Windows.Threading;

namespace AGROSMART_GUI.Views.Shared
{
    /// <summary>
    /// Lógica de interacción para BienvenidaPage.xaml
    /// </summary>
    public partial class BienvenidaPage : Window
    {
        private DispatcherTimer timer;

        public BienvenidaPage()
        {
            InitializeComponent();

            // Timer para cerrar después de 3 segundos
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            var mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }
    }
}
