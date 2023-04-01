using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner.Models
{
    internal class Map
    {
        public DemographicUnit[,] Matrix { get; set; }

        public double Distance(ILocation l1, ILocation l2)
        {
            return Math.Abs(l1.X - l2.X) + Math.Abs(l1.Y - l2.Y);
        }
    }
}
