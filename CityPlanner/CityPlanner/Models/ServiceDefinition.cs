using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner.Models
{
    public class ServiceDefinition
    {
        public string Name { get; set; }

        public string Type { get; set; }
        public int PopulationCapacity { get; set; }

        public bool Shown { get; set; }

        public override string ToString()
        {
            return Type;
        }
    }
}
