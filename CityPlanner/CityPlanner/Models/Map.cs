using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner.Models
{
    internal class Map
    {
        public int Width { get => Matrix.GetLength(0); }
        public int Height { get => Matrix.GetLength(1); }
        public DemographicUnit[,] Matrix { get; set; }

        public double Distance(ILocation l1, ILocation l2)
        {
            return Math.Abs(l1.X - l2.X) + Math.Abs(l1.Y - l2.Y);
        }

        public void LoadFromCsv(string filepath)
        {
            using(StreamReader sr = new(filepath))
            {
                List<DemographicUnit[]> cols = new();
                int y = 0;
                int width = 0;
                int height = 0; 
                while (!sr.EndOfStream) 
                {
                    string line = sr.ReadLine();
                    string[] lineArr = line.Split(';');
                    width = lineArr.Length;
                    DemographicUnit[] row = new DemographicUnit[lineArr.Length];
                    int x = 0;
                    for (int i = 0; i < lineArr.Length; i++)
                    {
                        string[] demoUnitVals = lineArr[i].Split('/');
                        row[i] = new DemographicUnit()
                        {
                            Y = y,
                            X = x,
                            IncomeAvg = double.Parse(demoUnitVals[0]),
                            Population = int.Parse(demoUnitVals[1]),
                        };
                        x++;
                    }
                    cols.Add(row);
                    y++;
                }
                Matrix = new DemographicUnit[width, height];
            }
        }
    }
}
