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
            // x, y = proměnné pozice služby
            // užitková funkce = -sum(vzdálenost(oblast,x,y))

            Variable x = solver.MakeNumVar(0, map.Width, "x");
            Variable y = solver.MakeNumVar(0, map.Height, "y");

            solver.Maximize(objective_fce(areas, x, y);

            var pole = areas.Select(unit => Math.Abs(unit.X - x) + Math.Abs(unit.Y - y));
        }
        private LinearExpr objective_fce(List<DemographicUnit> areas, Variable x, Variable y)
        {
            


        }
        private float distance(DemographicUnit unit, float x, float y)
        {
            return Math.Abs(unit.X - x) + Math.Abs(unit.Y - y);
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
