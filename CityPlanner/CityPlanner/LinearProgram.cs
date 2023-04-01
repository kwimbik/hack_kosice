using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
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

        private void makeLP()
        {
            Variable x = solver.MakeNumVar(0.0, map.Width, "x");
            Variable y = solver.MakeNumVar(0.0, map.Height, "y");

            solver.Maximize(Functions.utility(areas, new ServiceLocation {X = x, Y = y});
        }

        private void AddVariables()
        {
            Variable x = solver.MakeNumVar(0.0, map.Width, "x");
            Variable y = solver.MakeNumVar(0.0, map.Height, "y");
        }

        private void AddConstraints() // could depend on service type
        {
            //omezení mapy
        }

        private void AddObjectiveFunction() 
        {
            
        }

       



    }
}
