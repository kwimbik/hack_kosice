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
            string file = @"..\..\..\..\..\Datasets\adresne_body_byty_KE.csv";

            float minX = int.MaxValue;
            float maxX = int.MinValue;
            float minY = int.MaxValue;
            float maxY = int.MinValue;

            using (StreamReader sr = new StreamReader(file))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                string[] line = sr.ReadLine().Split(',');
                while ((currentLine = sr.ReadLine()) != null)
                {
                    line = currentLine.Split(',');

                    //map.Matrix[Convert.ToInt32(line[3]), Convert.ToInt32(line[4])].Population = Convert.ToInt32(line[12]);

                    if (float.Parse(line[3]) < minX) minX = float.Parse(line[3]);
                    if (float.Parse(line[3]) > maxX) maxX = float.Parse(line[3]);
                    if (float.Parse(line[4]) < minY) minY = float.Parse(line[4]);
                    if (float.Parse(line[4]) > maxY) maxY = float.Parse(line[4]);
                }

                sr.Close();
            }

            map.Max_X = maxX; 
            map.Max_Y = maxY;
            map.Min_X = minX;
            map.Min_Y = minY;

            map.Unit_X = (maxX - minX) / map.Width;
            map.Unit_Y = (maxY - minY) / map.Height;

            using (StreamReader sr = new StreamReader(file))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                string[] line = sr.ReadLine().Split(',');
                while ((currentLine = sr.ReadLine()) != null)
                {
                    line = currentLine.Split(',');

                    float x = (float.Parse(line[3]) - minX)/map.Unit_X - 1;
                    float y = (float.Parse(line[4]) - minY) / map.Unit_Y - 1;

                    map.Matrix[Math.Max(0, Convert.ToInt32(x)), Math.Max(0, Convert.ToInt32(y))].Population = (int)Convert.ToDouble(line[12]);
                }

                sr.Close();
            }
            return map;
        }
    }
}
