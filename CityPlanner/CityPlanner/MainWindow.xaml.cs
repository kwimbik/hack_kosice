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

        #region Contsructors

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

        private void DrawMap(Map map)
        {
            Image image = new();
            DrawingImage drawingImage = new();
            DrawingGroup drawingGroup = new();

            const int demoUnitWidth = 5;
            const int demoUnitHeight = 5;

            int maxPopulation = 0;
            for (int i = 0; i < map.Width; i++)
                for (int j = 0; j < map.Height; j++)
                    if (map.Matrix[i,j].Population > maxPopulation) maxPopulation = map.Matrix[i,j].Population;

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    int x = i * demoUnitWidth;
                    int y = j * demoUnitHeight;

                    // Draw demographic unit
                    int population = map.Matrix[i,j].Population;
                    Color color = Color.FromRgb((byte)(255 * population / maxPopulation), 0, 0);
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


        private void DrawRectangle(double x, double y, double width, double height, Color strokeColor, int strokeThickness, Color fillColor)
        {
            Rectangle rect = new()
            {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(strokeColor),
                StrokeThickness = strokeThickness,
                Fill = new SolidColorBrush(fillColor),
            };
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            cMMap.Children.Add(rect);
        }

        private void DrawLine(double x1, double x2, double y1, double y2, Color color, int thickness)
        {
            Line line = new()
            {
                X1 = x1,
                X2 = x2,
                Y1 = y1,
                Y2 = y2,
                Stroke = new SolidColorBrush(color),
                StrokeThickness = thickness
            };
            cMMap.Children.Add(line);
        }


        private void DrawServicesMap(List<ServiceLocation> locations)
        {
            cMMap.Children.Clear();

            Image image = new();
            DrawingImage drawingImage = new();
            DrawingGroup drawingGroup = new();

            const int demoUnitWidth = 5;
            const int demoUnitHeight = 5;


            float[,] stats = Stats.getServiceStats(map, locations);
            const float maxOkDistance = 15;

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    double x = i * demoUnitWidth;
                    double y = j * demoUnitHeight;

                    // Draw demographic unit
                    float stat = stats[i, j];
                    Color color;
                    if (stat > maxOkDistance)
                    {
                        color = Color.FromRgb(255, 0, 0);
                    }
                    else
                    {
                        float normalizedDistance = stat / maxOkDistance;
                        color = Color.FromRgb((byte)(normalizedDistance * 255), (byte)((1 - normalizedDistance) * 255), 0);
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

        #region Form

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgServices.ItemsSource = Model.ServiceDefinitions;

            InitMap();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            List<ServiceLocation> locations = new List<ServiceLocation>();
            List<string> selected = new List<string>();

            foreach (ServiceDefinition service in Model.ServiceDefinitions.Where(s => s.shown == true))
            {
                selected.Add(service.Type);
            }

            foreach (var type in selected)
            {
                locations.AddRange(map.Services.Where(s => s.Definition.Type == type).ToList());
            }

            DrawServicesMap(locations);
        }

        private void Stats_Click(object sender, RoutedEventArgs e)
        {
            string file = @"..\..\..\..\..\maps\map_1.csv";
            
            HouseholdParser.parseHouseholds(map);
            ServiceParser.parseServices(map);
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
                            DrawServicesMap(best.Services.Select(s => new ServiceLocation() { X = (int)s.X, Y = (int)s.Y }).ToList());
                        }));
                    }
                };
                evo.Run();
            });
        }

        #endregion
    }
}
