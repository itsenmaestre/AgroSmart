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

namespace AGROSMART_GUI.Views.Empleado
{
    /// <summary>
    /// Lógica de interacción para EmpleadoView.xaml
    /// </summary>
    public partial class EmpleadoView : Window
    {
        AsignacionTareaService service = new AsignacionTareaService();
        int idEmpleado;

        public EmpleadoView(int idEmpleadoActual)
        {
            InitializeComponent();
            idEmpleado = idEmpleadoActual;
            CargarTareas();
        }

        void CargarTareas()
        {
            List<ASIGNACION_TAREA> lista = service.ListarPorEmpleado(idEmpleado);
            dgTareas.ItemsSource = lista; // tu DataGrid se llama dgTareas
        }

        private void BtnRegistrarAvance_Click(object sender, RoutedEventArgs e)
        {
            ASIGNACION_TAREA seleccionada = (ASIGNACION_TAREA)dgTareas.SelectedItem;
            if (seleccionada == null)
            {
                MessageBox.Show("Seleccione una tarea.");
                return;
            }

            RegistrarAvanceView v = new RegistrarAvanceView(seleccionada);
            v.ShowDialog();
            CargarTareas();
        }
    }
}
