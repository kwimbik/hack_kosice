using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityPlanner.Models;

namespace CityPlanner
{
    internal class TilePlacer
    {
        private static List<ServiceLocation> placeOne(Map map, List<ServiceLocation> locationsOnMap) 
        {
            (int, int) bestPlacement = (0, 0);
            float bestScore = Stats.getMapScore(map, locationsOnMap);

            for (int y = 0; y < map.Height; y++) 
            {
                for (int x = 0; x < map.Width; x++) 
                {
                    ServiceLocation newLocation = new ServiceLocation();
                    newLocation.X = x;
                    newLocation.Y = y;
                    locationsOnMap.Add(newLocation);
                    float newScore = Stats.getMapScore(map, locationsOnMap);
                    locationsOnMap.RemoveAt(locationsOnMap.Count - 1);

                    if (newScore <= bestScore) 
                    {
                        bestScore = newScore;
                        bestPlacement = (y, x);
                    }
                }
            }

            ServiceLocation bestLocation = new ServiceLocation();
            bestLocation.X = bestPlacement.Item2;
            bestLocation.Y = bestPlacement.Item1;
            locationsOnMap.Append(bestLocation);

            return locationsOnMap;
        }

        public static List<ServiceLocation> PlaceN(Map map, List<ServiceLocation> locationsOnMap, int n) 
        {
            if (n == 0) 
            {
                return locationsOnMap;
            }

            List<ServiceLocation> updatedLocations = PlaceN(map, locationsOnMap, n - 1);

            return placeOne(map, updatedLocations);
        }
    }
}
