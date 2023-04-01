﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner.Models
{
    public class DemographicUnit : ILocation
    {
        public int Population { get; set; }
        public double IncomeAvg { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
