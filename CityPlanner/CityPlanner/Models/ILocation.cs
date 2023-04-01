using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner.Models
{
    internal interface ILocation
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
