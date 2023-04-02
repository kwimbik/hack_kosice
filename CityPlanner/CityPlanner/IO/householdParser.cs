using CityPlanner.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner.IO
{
    internal class HouseholdParser
    {

        public static void ParseHouseholds(Map map)
        {
            string file = @"..\..\..\..\..\Datasets\adresne_body_byty_KE.csv";

            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            using (StreamReader sr = new StreamReader(file))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                string[] line = sr.ReadLine().Split(CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                while ((currentLine = sr.ReadLine()) != null)
                {
                    line = currentLine.Split(CultureInfo.CurrentCulture.TextInfo.ListSeparator);

                    //map.Matrix[Convert.ToInt32(line[3]), Convert.ToInt32(line[4])].Population = Convert.ToInt32(line[12]);

                    if (double.Parse(line[3], CultureInfo.InvariantCulture) < minX) minX = double.Parse(line[3], CultureInfo.InvariantCulture);
                    if (double.Parse(line[3], CultureInfo.InvariantCulture) > maxX) maxX = double.Parse(line[3], CultureInfo.InvariantCulture);
                    if (double.Parse(line[4], CultureInfo.InvariantCulture) < minY) minY = double.Parse(line[4], CultureInfo.InvariantCulture);
                    if (double.Parse(line[4], CultureInfo.InvariantCulture) > maxY) maxY = double.Parse(line[4], CultureInfo.InvariantCulture);
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
                string[] line = sr.ReadLine().Split(CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                while ((currentLine = sr.ReadLine()) != null)
                {
                    line = currentLine.Split(CultureInfo.CurrentCulture.TextInfo.ListSeparator);

                    double x = (double.Parse(line[3], CultureInfo.InvariantCulture) - minX)/map.Unit_X - 1;
                    double y = (double.Parse(line[4], CultureInfo.InvariantCulture) - minY) / map.Unit_Y - 1;

                    map.Matrix[Math.Max(0, Convert.ToInt32(x)), Math.Max(0, Convert.ToInt32(y))].Population += (int)Convert.ToDouble(line[12], CultureInfo.InvariantCulture);
                }

                sr.Close();
            }
        }
    }
}
