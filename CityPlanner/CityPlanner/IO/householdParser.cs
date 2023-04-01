using CityPlanner.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner.IO
{
    internal class householdParser
    {

        public static Map parseHouseholds(Map map)
        {
            string file = @"..\..\..\..\..\maps\adresne_body_byty_KE.csv";

            using (StreamReader sr = new StreamReader(file))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                string[] line = sr.ReadLine().Split(',');
                while ((currentLine = sr.ReadLine()) != null)
                {
                    line = currentLine.Split(',');

                    map.Matrix[Convert.ToInt32(line[3]), Convert.ToInt32(line[4])].Population = Convert.ToInt32(line[12]);
                }
            }

            return map;
        }
    }
}
