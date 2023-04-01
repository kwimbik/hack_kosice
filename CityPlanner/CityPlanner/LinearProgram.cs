using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using CityPlanner.Models;
using Google.OrTools.LinearSolver;

namespace CityPlanner
{
    internal class LinearProgram
    {

        private Solver solver;
        private List<DemographicUnit> areas;
        private ServiceLocation service;
        private Map map;

        public LinearProgram(List<DemographicUnit> areas, ServiceLocation service, Map map) 
        {
            solver = Solver.CreateSolver("GLOP");
            this.areas = areas;  
            this.service = service;
            this.map = map;

        }

        private void AddVariables()
        {
            Variable x = solver.MakeNumVar(0.0, map.Matrix.Width, "x");
            Variable y = solver.MakeNumVar(0.0, map.Matrix.Height, "y");

        }

        private void AddConstraints() // could depend on service type
        {
            //omezení mapy
        }

       



    }
}
