using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;

namespace CityPlanner
{
    internal class LinearProgram
    {
        //private Functions fce = new Functions();

        public LinearProgram() 
        {  
            Solver solver = Solver.CreateSolver("GLOP");
            if (solver is null)
            {
                return;
            }
        }

       



    }
}
