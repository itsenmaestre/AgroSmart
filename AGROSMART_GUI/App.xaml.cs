using AGROSMART_GUI.Views.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AGROSMART_GUI
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Si BienvenidaPage es Window (lo es en tu captura):
            var win = new BienvenidaPage();
            win.Show();

            // Si en algún momento la vuelves Page, usa esto:
            // var nav = new NavigationWindow { ShowsNavigationUI = false, Content = new BienvenidaPage(), Width = 900, Height = 600 };
            // nav.Show();
        }
    }
}
