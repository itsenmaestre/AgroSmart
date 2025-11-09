using AGROSMART_BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            // Registrar el converter
            this.Resources.Add("EstadoToColorConverter", new EstadoToColorConverter());

            CargarTareas();
        }

        private void CargarTareas()
        {
            try
            {
                var asignaciones = _asigService.ListarPorEmpleado(_idEmpleado);
                List<TareaItem> items = new List<TareaItem>();

                int pendientes = 0;
                int enEjecucion = 0;
                int finalizadas = 0;

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

                    // Contar por estado
                    if (a.ESTADO == "PENDIENTE") pendientes++;
                    else if (a.ESTADO == "EN_EJECUCION") enEjecucion++;
                    else if (a.ESTADO == "FINALIZADA") finalizadas++;
                }

                lstTareas.ItemsSource = items;

                // Actualizar contadores
                txtTotalTareas.Text = items.Count.ToString();
                txtPendientes.Text = pendientes.ToString();
                txtEjecucion.Text = enEjecucion.ToString();
                txtFinalizadas.Text = finalizadas.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar tareas: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Converter para colores de estado
    public class EstadoToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null) return Colors.Gray;

            string estado = values[0].ToString();
            switch (estado)
            {
                case "PENDIENTE":
                    return Color.FromRgb(243, 156, 18); // Naranja
                case "EN_EJECUCION":
                    return Color.FromRgb(33, 150, 243); // Azul
                case "FINALIZADA":
                    return Color.FromRgb(76, 175, 80); // Verde
                default:
                    return Colors.Gray;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
