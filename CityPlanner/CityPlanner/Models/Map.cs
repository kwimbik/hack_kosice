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

        public float Max_X { get; set; }
        public float Min_X { get; set; }
        public float Max_Y { get; set; }
        public float Min_Y { get; set; }

        public float Unit_X { get; set; }
        public float Unit_Y { get; set; }

        public List<ServiceLocation> Services { get; set; } = new();
        public DemographicUnit[,] Matrix { get; set; }

        public double Distance(ILocation l1, ILocation l2)
        {
            return Math.Abs(l1.X - l2.X) + Math.Abs(l1.Y - l2.Y);
        }
    }
}
