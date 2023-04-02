using CityPlanner.IO;
using CityPlanner.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace CityPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Props

        public MainWindowModel Model => (MainWindowModel)DataContext;

        private int demoUnitWidth => (int)cMMap.ActualWidth / map.Width;
        private int demoUnitHeight => (int)cMMap.ActualHeight / map.Height;


        #endregion

        #region Fields

        private Map map = new() { Matrix = new DemographicUnit[100, 100] };
        private Dictionary<IntCords, DemographicUnit> _mapCords = new();

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Manhattan map

        private void InitMap()
        {
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    map.Matrix[x, y].X = x;
                    map.Matrix[x, y].Y = y;
                    map.Matrix[x, y].Population = 0;
                }
            }

            HouseholdParser.ParseHouseholds(map);
        }

        private void DrawPopulationMap()
        {
            cMMap.Children.Clear();
            _mapCords.Clear();

            Image image = new();
            DrawingImage drawingImage = new();
            DrawingGroup drawingGroup = new();

           
            int maxPopulation = 0;
            for (int i = 0; i < map.Width; i++)
                for (int j = 0; j < map.Height; j++)
                    if (map.Matrix[i, j].Population > maxPopulation) maxPopulation = map.Matrix[i, j].Population;

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    int x = i * demoUnitWidth;
                    int y = j * demoUnitHeight;

                    // Draw demographic unit
                    int population = map.Matrix[i, j].Population;
                    Color color = Color.FromArgb((byte)(50 + (255 * (double)population / maxPopulation)), (byte)(255 - 255 * 10000 * (double)population / maxPopulation), 0, 0);
                    if ((double)population / maxPopulation <= 1 / 10000) 
                    {
                        color = Color.FromArgb(0, 0, 0, 0);
                    }
                    GeometryDrawing gd = new()
                    {
                        Geometry = new RectangleGeometry(new Rect() { X = x, Y = y, Width = demoUnitWidth, Height = demoUnitHeight }),
                        Brush = new SolidColorBrush(color)
                    };
                    drawingGroup.Children.Add(gd);
                    _mapCords.Add(new IntCords(x, y), map.Matrix[i, j]);
                }
            }

            drawingImage.Drawing = drawingGroup;
            image.Source = drawingImage;
            cMMap.Children.Add(image);
        }

        private void DrawServicesMap(List<ServiceLocation> locations, bool clearChildern = true)
        {
            if (clearChildern) cMMap.Children.Clear();

            Image image = new();
            image.Opacity = 0.7;
            DrawingImage drawingImage = new();
            DrawingGroup drawingGroup = new();

          
            float[,] stats = Stats.getServiceStats(map, locations);
            const float maxOkDistance = 8;

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    double x = i * demoUnitWidth;
                    double y = j * demoUnitHeight;

                    // Draw demographic unit
                    float stat = stats[i, j];

                    Color color = Color.FromArgb(0, 0, 0, 0);

                    if (stat <= maxOkDistance)
                    {

                        float normalizedDistance = stat / maxOkDistance;
                        color = Color.FromArgb((byte)(32 + (Math.Min(1,(1.15 - normalizedDistance)) * 223)), 0, 255, 0);

                    }

                    GeometryDrawing gd = new()
                    {
                        Geometry = new RectangleGeometry(new Rect() { X = x, Y = y, Width = demoUnitWidth, Height = demoUnitHeight }),
                        Brush = new SolidColorBrush(color)
                    };
                    drawingGroup.Children.Add(gd);
                }
            }

            drawingImage.Drawing = drawingGroup;
            image.Source = drawingImage;
            cMMap.Children.Add(image);
        }

        #endregion

        #region Services

        private List<ServiceLocation> SelectedServiceLocations()
        {
            List<ServiceLocation> locations = new List<ServiceLocation>();
            List<string> selected = new List<string>();

            foreach (ServiceDefinition service in Model.ServiceDefinitions.Where(s => s.Shown == true))
            {
                selected.Add(service.Type);
            }

            foreach (var type in selected)
            {
                foreach (var service in map.Services)
                {
                    if (service.Definition.Type == type) locations.Add(service);
                }
            }
            return locations;
        }

        #endregion

        #region Form

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ResizeMode = ResizeMode.NoResize;
            dgServices.ItemsSource = Model.ServiceDefinitions;

            InitMap();

            var imageKosice = new BitmapImage(new Uri(@"..\..\..\..\..\images\kosice.png", UriKind.Relative));
            cMMap.Background = new ImageBrush(imageKosice);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            DrawPopulationMap();
            DrawServicesMap(SelectedServiceLocations(), clearChildern: false);
            dgPeopleInfo.ItemsSource = Display_info();
        }

        private void NewService_Click(object sender, RoutedEventArgs e)
        {
            NewServiceWindow nsw = new NewServiceWindow(map, this);
            nsw.Show();
            this.Refresh_Click(sender,e);
        }

        private ObservableCollection<InfoView> Display_info()
        {
            ObservableCollection<InfoView>  info = new ObservableCollection<InfoView>();

            foreach (ServiceDefinition service in Model.ServiceDefinitions.Where(s => s.Shown == true))
            {
                var locations = map.Services.Where(s => s.Definition.Type == service.Type).ToList();

                var people = Functions.getPeopleStats(map, locations);
                info.Add(new InfoView { people = people.Item1, value = $"out of 15m reach for {service.Type}" });
                info.Add(new InfoView { people = people.Item2, value = $"within 15m reach for {service.Type}" });
            }
            return info;
        }

        private void LoadPopulation_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            //HouseholdParser.ParseHouseholds(map);
            DrawPopulationMap();
            Cursor = Cursors.Arrow;
        }

        private void LoadServices_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            ServiceParser.ParseServices(map);
            DrawPopulationMap();
            DrawServicesMap(SelectedServiceLocations(), clearChildern: false);
            Cursor = Cursors.Arrow;
        }

        private void Evo_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Evolution evo = new();
                evo.GenerationEvent += (g, population, fitnesses) =>
                {
                    if (g % 10 == 0)
                    {
                        int bestInPopIndex = Array.IndexOf(fitnesses, fitnesses.Max());
                        Individual best = population[bestInPopIndex];

                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            DrawPopulationMap();
                            DrawServicesMap(SelectedServiceLocations(), clearChildern: false);
                            DrawServicesMap(best.Services.Select(s => new ServiceLocation() { X = (int)s.X, Y = (int)s.Y }).ToList(), clearChildern: false);
                        }));
                    }
                };
                evo.Run(map, 50, 8, 1000);
            });
        }

        private void cMMap_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point loc = e.GetPosition(cMMap);
            int x = (int)loc.X - (int)loc.X % demoUnitWidth;
            int y = (int)loc.Y - (int)loc.Y % demoUnitHeight;
            IntCords cords = new(x, y);
            if (_mapCords.ContainsKey(cords))
            {
                DemographicUnit du = _mapCords[cords];
                ObservableCollection<ServiceDistanceView> serviceDistances = new ObservableCollection<ServiceDistanceView>();

                List<string> selected = new List<string>();

                foreach (ServiceDefinition service in Model.ServiceDefinitions)
                {
                    selected.Add(service.Type);
                }

                foreach (var type in selected)
                {
                    List<ServiceLocation> locations = map.Services.Where(s => s.Definition.Type == type).ToList();
                    serviceDistances.Add(new ServiceDistanceView { serviceType = type, distance = Functions.getLocationStatus(du, locations) });
                }
                dgPeopleInfo.ItemsSource = serviceDistances;
            }
            else
            {
                //throw new Exception("!!!!!");
            }
        }
        

        #endregion


    }
}
