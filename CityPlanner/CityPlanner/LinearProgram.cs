using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;

namespace CityPlanner
{
    internal class LinearProgram
    {

        private Solver Solver;
        private List<Area> Areas;
        private Service Service;

        public LinearProgram(List<Area> areas, Service service) 
        {
            Solver = Solver.CreateSolver("GLOP");
            Areas = areas;  
            Service = service;
        }

        private void AddVariables()
        {

        }

        private void AddConstraints() // could depend on service type
        {

        }

       



    }
}
