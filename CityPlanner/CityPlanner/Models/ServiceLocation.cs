using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner.Models
{
    internal class ServiceLocation : ILocation
    {
        public ServiceDefinition Definition { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
