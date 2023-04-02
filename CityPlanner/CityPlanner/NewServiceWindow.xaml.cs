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

            ComboBox types_cb = new ComboBox
            {
                Text = "Select Service type",
            };

            foreach (var s in map.Services)
            {
                if (types_cb.Items.Contains(s.Definition.Type) == false) 
                {
                    types_cb.Items.Add(s.Definition.Type);
                }
            }
            sp.Children.Add(types_cb);
            types_cb.SelectedIndex = 0;

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
                try
                {
                    ServiceLocation sl = new ServiceLocation { X = int.Parse(Xcoord_tb.Text), Y = int.Parse(Ycoord_tb.Text), Definition = new ServiceDefinition { Type = types_cb.Text, Shown = false } };
                    map.Services.Add(sl);
                }
                catch (Exception)
                {
                    MessageBox.Show("please select valid location");
                }
                
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
