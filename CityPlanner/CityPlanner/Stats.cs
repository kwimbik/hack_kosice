using CityPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner
{
    public class Stats
    {
        public static float[,] getServiceStats(Map map, List<ServiceLocation> services) 
        {
            float[,] serviceStats = new float[map.Width, map.Height];

            for (int x = 0; x < map.Width; x++) 
            {
                for (int y = 0; y < map.Width; y++) 
                {
                    float quality = Functions.getLocationStatus(map.Matrix[y,x], services);
                    serviceStats[x, y] = quality;
                }
            }

            return serviceStats;
        }
    }
}
