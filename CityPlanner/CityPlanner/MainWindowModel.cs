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
        public ObservableCollection<ServiceDefinition> ServiceDefinitions { get; set; } = new ObservableCollection<ServiceDefinition> 
        {
        new ServiceDefinition {Type =  "ambulancia zubného lekárstva"},
        new ServiceDefinition {Type =  "Všeobecná ambulancia pre deti"},
        new ServiceDefinition {Type =  "Pošty"},
        new ServiceDefinition {Type =   "restaurant"},
        new ServiceDefinition {Type =  "playground"},
        new ServiceDefinition {Type =  "Vybehy_psy"},
        new ServiceDefinition {Type =  "chemist"},
        new ServiceDefinition {Type =  "MHD"},
        new ServiceDefinition {Type =  "fitness"},
        new ServiceDefinition {Type =  "bar"},
        new ServiceDefinition {Type =  "pub"},
        new ServiceDefinition {Type =  "fast_food"},
        new ServiceDefinition {Type =  "MS"},
        new ServiceDefinition {Type =  "fitness"},
        new ServiceDefinition {Type =  "convenience"},
        new ServiceDefinition {Type =  "cafe"},
        new ServiceDefinition {Type =  "Všeobecná ambulancia pre dospelých"},
        new ServiceDefinition {Type =  "Pošty"},
        };

        public MainWindowModel() 
        {
          
        }
    }
}
