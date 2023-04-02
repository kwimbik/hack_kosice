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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CityPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Props

        public MainWindowModel Model => (MainWindowModel)DataContext;

        #endregion

        #region Fields

        private Map map = new() { Matrix = new DemographicUnit[100, 100] };

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
                    map.Matrix[x, y].Population = 1;
                }
            }
        }

        private void DrawPopulationMap()
        {
            cMMap.Children.Clear();

            Image image = new();
            DrawingImage drawingImage = new();
            DrawingGroup drawingGroup = new();

            double demoUnitWidth = cMMap.ActualWidth / map.Width;
            double demoUnitHeight = cMMap.ActualHeight / map.Height;

            int maxPopulation = 0;
            for (int i = 0; i < map.Width; i++)
                for (int j = 0; j < map.Height; j++)
                    if (map.Matrix[i, j].Population > maxPopulation) maxPopulation = map.Matrix[i, j].Population;

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    double x = i * demoUnitWidth;
                    double y = j * demoUnitHeight;

                    // Draw demographic unit
                    int population = map.Matrix[i, j].Population;
                    Color color = Color.FromRgb(0, (byte)(255 * (double)population / maxPopulation), 0);
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

        private void DrawServicesMap(List<ServiceLocation> locations, bool clearChildern = true)
        {
            if (clearChildern) cMMap.Children.Clear();

            Image image = new();
            image.Opacity = 0.4;
            DrawingImage drawingImage = new();
            DrawingGroup drawingGroup = new();

            double demoUnitWidth = cMMap.ActualWidth / map.Width;
            double demoUnitHeight = cMMap.ActualHeight / map.Height;


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

                    if (stat <= maxOkDistance)
                    {

                        float normalizedDistance = stat / maxOkDistance;
                        Color color = Color.FromRgb((byte)(normalizedDistance * 255), (byte)((1 - normalizedDistance) * 255), 0);

                        GeometryDrawing gd = new()
                        {
                            Geometry = new RectangleGeometry(new Rect() { X = x, Y = y, Width = demoUnitWidth, Height = demoUnitHeight }),
                            Brush = new SolidColorBrush(color)
                        };
                        drawingGroup.Children.Add(gd);
                    }
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
                locations.AddRange(map.Services.Where(s => s.Definition.Type == type).ToList());
            }
            return locations;
        }

        #endregion

        #region Form

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgServices.ItemsSource = Model.ServiceDefinitions;

            InitMap();

            var imageKosice = new BitmapImage(new Uri(@"..\..\..\..\..\images\kosice.png", UriKind.Relative));
            //cMMap.Background = new ImageBrush(imageKosice);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            DrawPopulationMap();
            DrawServicesMap(SelectedServiceLocations(), clearChildern: false);
        }

        private void LoadPopulation_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            HouseholdParser.ParseHouseholds(map);
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
                            DrawServicesMap(best.Services.Select(s => new ServiceLocation() { X = (int)s.X, Y = (int)s.Y }).ToList(), clearChildern: false);
                        }));
                    }
                };
                evo.Run(map, 50, 2, 1000);
            });
        }

        #endregion
    }
}
