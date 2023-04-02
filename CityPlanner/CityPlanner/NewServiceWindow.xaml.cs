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
using System.Windows.Shapes;

namespace CityPlanner
{
    /// <summary>
    /// Interaction logic for NewServiceWindow.xaml
    /// </summary>
    public partial class NewServiceWindow : Window
    {
        Grid g = new Grid();

        public NewServiceWindow(Map map, MainWindow mw)
        {
            InitializeComponent();
            createComponent(map, mw);
            this.Content = g;
        }

        private void createComponent(Map map, MainWindow mw)
        {
            StackPanel sp = new StackPanel();
            g.Children.Add(sp);

            TextBox service_tb = new TextBox
            {
                Text = "Service",
            };
            sp.Children.Add(service_tb);
            TextBox Xcoord_tb = new TextBox
            {
                Text = "X coordinate",
            };
            sp.Children.Add(Xcoord_tb);
            TextBox Ycoord_tb = new TextBox
            {
                Text = "Y coordinate",
            };
            sp.Children.Add(Ycoord_tb);

            Button Confirm_tb = new Button
            {
                Content = "Confirm"
            };

            Confirm_tb.Click += (s, e) =>
            {
                ServiceLocation sl = new ServiceLocation { X = int.Parse(Xcoord_tb.Text), Y = int.Parse(Ycoord_tb.Text), Definition = new ServiceDefinition { Type = service_tb.Text, Shown = false } };
                map.Services.Add(sl);
            };
            sp.Children.Add(Confirm_tb);

            Button Cancelbtn = new Button
            {
                Content = "Cancel"
            };

            Confirm_tb.Click += (s, e) =>
            {
                this.Close();
            };
        }
    }
}
