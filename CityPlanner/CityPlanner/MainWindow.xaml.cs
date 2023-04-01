﻿using CityPlanner.Models;
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

        private Dictionary<(int x, int y), GeometryDrawing> _geometry = new();

        #endregion

        #region Contsructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Manhattan map

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
                    _geometry.Add((x, y), gd);
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


        #endregion

        #region Form

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgServices.ItemsSource = Model.ServiceDefinitions;
            lbServices.ItemsSource = Model.ServiceDefinitions;
            

            string file = @"..\..\..\..\..\maps\map_1.csv";
            Map map = new();
            map.LoadFromCsv(file);

            DrawMap(map);
        }

        private void ImportMap_Click(object sender, RoutedEventArgs e)
        {

        }
        private void cMMap_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(cMMap);
            int x = (int)position.X;
            int y = (int)position.Y;

            var key = (x - x % 5, y - y % 5);
            GeometryDrawing gd = _geometry[key];
            //gd.Brush = new SolidColorBrush(Colors.Beige);
        }

        #endregion

<<<<<<< Updated upstream

=======
        private void run_btn_Click(object sender, RoutedEventArgs e)
        {
            //TODO BARA
        }

        private void Stats_Click(object sender, RoutedEventArgs e)
        {
            string file = @"..\..\..\..\..\maps\map_1.csv";
            Map map = new();
            map.LoadFromCsv(file);

            ServiceDefinition definition = new ServiceDefinition();
            ServiceLocation s1 = new ServiceLocation();
            s1.X = 1;
            s1.Y = 1;
            ServiceLocation s2 = new ServiceLocation();
            s1.X = 1;
            s1.Y = 131;
            ServiceLocation s3 = new ServiceLocation();
            s1.X = 162;
            s1.Y = 1;
            ServiceLocation s4 = new ServiceLocation();
            s1.X = 250;
            s1.Y = 125;

            List<ServiceLocation> locations = new List<ServiceLocation>() { s1, s2, s3, s4 };

            // Vykradeno z Draw Map

            Image image = new();
            DrawingImage drawingImage = new();
            DrawingGroup drawingGroup = new();

            const int demoUnitWidth = 5;
            const int demoUnitHeight = 5;

            float[,] stats = Stats.getServiceStats(map, locations);

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    double x = i * demoUnitWidth;
                    double y = j * demoUnitHeight;

                    // Draw demographic unit
                    float stat = stats[i, j];
                    Color color;
                    switch (stat) 
                    {
                        case 0:
                            color = Color.FromRgb(0, 255, 0);
                            break;
                        case 1:
                            color = Color.FromRgb(255, 255, 0);
                            break;
                        default:
                            color = Color.FromRgb(255, 0, 0);
                            break;
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
>>>>>>> Stashed changes
    }
}
