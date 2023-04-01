using CityPlanner.Models;
using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner
{
    public class Stats
    {
        public static float getMapScore(Map map, List<ServiceLocation> services) 
        {
            float[,] scoreMatrix = getServiceStats(map, services);

            float totalScore = 0;
            for (int y = 0; y < scoreMatrix.GetLength(0); y++) 
            {
                float rowScore = 0;
                for (int x = 0; x < scoreMatrix.GetLength(1); x++) 
                {
                    rowScore += scoreMatrix[y, x];
                }
                totalScore += rowScore;
            }

            return totalScore;
        }

        public static float[,] getServiceStats(Map map, List<ServiceLocation> services) 
        {
            float[,] serviceStats = new float[map.Width, map.Height];

            for (int y = 0; y < map.Width; y++) 
            {
                for (int x = 0; x < map.Height; x++) 
                {
                    float quality = Functions.getLocationStatus(map.Matrix[y,x], services);
                    serviceStats[y, x] = quality;
                }
            }

            return serviceStats;
        }
    }
}
