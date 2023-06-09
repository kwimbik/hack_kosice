﻿using CityPlanner.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CityPlanner
{
    public static class Functions
    {
        public static float utility(DemographicUnit o, ServiceLocation t)
        {

            return -Math.Max(0, dist(o, t) - 15);
        }

        public static float dist(ILocation p1, ILocation p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

        public static (int,int) getPeopleStats(Map map, List<ServiceLocation> location)
        {
            int peopleOutOfReach = 0;
            int peopleInReach = 0;
            for (int i = 0; i < map.Width; i++)
            {
                for(int j = 0; j < map.Height; j++)
                {
                    float minDist = float.MaxValue;
                    foreach(ServiceLocation l in location) 
                    {
                        minDist = Math.Min(minDist, dist(l, map.Matrix[i, j]));
                    }
                    if (minDist > 15)
                    {
                        peopleOutOfReach += map.Matrix[i, j].Population;
                    }
                    else peopleInReach += map.Matrix[i, j].Population;
                }
            }
            return (peopleOutOfReach, peopleInReach);
        }

        public static float getLocationStatus(DemographicUnit o, List<ServiceLocation> services)
        {
            //float minDist = int.MaxValue;
            //foreach (ServiceLocation s in services)
            //{
            //    float distance = dist(o, s);
            //    if (distance < minDist) minDist = distance;
            // }

            //if (minDist > -1) return 0; //green, tohle je dobry
            //else if (minDist > -5) return 1; //tohle oranzovy
            //else return 2; //tohle je red, spatny

            //const float okTreshold = 20; // TODO: Is this a kilometer? Who knows???
            //const float almostOkTreshold = 50;

            float minDistance = float.MaxValue;
            foreach (ServiceLocation s in services) 
            {
                float distance = dist(o, s);
                if (distance < minDistance) 
                {
                    minDistance = distance;
                }
            }

            return minDistance;
        }
    }
}
