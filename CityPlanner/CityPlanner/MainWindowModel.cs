using CityPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner
{
    public class MainWindowModel
    {
        public ObservableCollection<ServiceDefinition> ServiceDefinitions { get; set; } = new();

        public MainWindowModel() 
        {
            ServiceDefinitions.Add(new ServiceDefinition() { Name = "Hospital" });
            ServiceDefinitions.Add(new ServiceDefinition() { Name = "KFC" });
            ServiceDefinitions.Add(new ServiceDefinition() { Name = "Cemetery" });
            ServiceDefinitions.Add(new ServiceDefinition() { Name = "Church" });
            ServiceDefinitions.Add(new ServiceDefinition() { Name = "Post office" });
        }
    }
}
