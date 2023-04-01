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
            float[,] serviceStats = new float[map.Height, map.Width];

            for (int y = 0; y < map.Height; y++) 
            {
                for (int x = 0; x < map.Width; x++) 
                {
                    float quality = Functions.getLocationStatus(map.Matrix[y,x], services);
                    serviceStats[y, x] = quality;
                }
            }

            return serviceStats;
        }
    }
}
