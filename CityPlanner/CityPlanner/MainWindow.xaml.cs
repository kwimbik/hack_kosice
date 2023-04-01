using CityPlanner.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CityPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Contsructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Manhattan map

        private void DrawManhattanMap()
        {
        }

        private void DrawManhattanBackground(Map map)
        {
            Image image = new();
            DrawingImage drawingImage = new();
            DrawingGroup drawingGroup = new();

            const int demoUnitWidth = 5;
            const int demoUnitHeight = 5;

            int width = map.Matrix.GetLength(0);
            int height = map.Matrix.GetLength(1);

            int maxPopulation = map.Matrix.Cast<int>().Max();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    double x = i * demoUnitWidth;
                    double y = j * demoUnitHeight;

                    // Draw demographic unit
                    int population = map.Matrix[i, j].Population;
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


        #endregion

        #region Form

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DrawManhattanMap();
        }

        private void ImportMap_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }
}
