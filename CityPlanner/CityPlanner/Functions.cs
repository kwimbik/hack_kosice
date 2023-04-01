using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CityPlanner
{
    public static class Functions
    {
        public static float utility(Oblast o, Sluzba t)
        {
            int sum = 0;
            foreach (Budova b in o.budovy)
            {
                sum += Math.Max(0, dist(b, t) - 15);
            }

            return -sum*1/(o.budovy.Count);
        }

        public static float dist(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }
    }
}
