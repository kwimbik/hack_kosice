using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner.Models
{
    public class Map
    {
        public int Width { get => Matrix.GetLength(0); }
        public int Height { get => Matrix.GetLength(1); }

        public double Max_X { get => 21.360399; }
        public double Min_X { get => 21.174135; }
        public double Max_Y { get => 48.771657; }
        public double Min_Y { get => 48.671984; }

        public double Unit_X { get; set; }
        public double Unit_Y { get; set; }

        public List<ServiceLocation> Services { get; set; } = new();
        public DemographicUnit[,] Matrix { get; set; }

        public double Distance(ILocation l1, ILocation l2)
        {
            return Math.Abs(l1.X - l2.X) + Math.Abs(l1.Y - l2.Y);
        }
    }
}
