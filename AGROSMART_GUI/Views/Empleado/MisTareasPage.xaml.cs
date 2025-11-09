using AGROSMART_BLL;
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
    /// Lógica de interacción para MisTareasPage.xaml
    /// </summary>
    public partial class MisTareasPage : Page
    {
        private readonly AsignacionTareaService _asigService = new AsignacionTareaService();
        private readonly TareaService _tareaService = new TareaService();
        private readonly int _idEmpleado;

        private class TareaItem
        {
            public int Codigo { get; set; }
            public string Estado { get; set; }
            public string FechaProgramada { get; set; }
            public string HorasAcumuladas { get; set; }
        }

        public MisTareasPage(int idEmpleado)
        {
            InitializeComponent();
            _idEmpleado = idEmpleado;
            CargarTareas();
        }

        private void CargarTareas()
        {
            try
            {
                var asignaciones = _asigService.ListarPorEmpleado(_idEmpleado);
                List<TareaItem> items = new List<TareaItem>();

                foreach (var a in asignaciones)
                {
                    var fecha = _tareaService.ObtenerFechaProgramada(a.ID_TAREA);

                    items.Add(new TareaItem
                    {
                        Codigo = a.ID_TAREA,
                        Estado = a.ESTADO,
                        FechaProgramada = fecha.HasValue ? fecha.Value.ToString("dd/MM/yyyy") : "-",
                        HorasAcumuladas = a.HORAS_TRABAJADAS.HasValue ? a.HORAS_TRABAJADAS.Value.ToString("0.##") : "0"
                    });
                }

                lstTareas.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar tareas: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
