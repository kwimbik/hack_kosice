using CityPlanner.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace CityPlanner.IO
{
    public static class serviceParser
    {

        public static List<ServiceLocation> parseServices()
        {
            List<string> types = new List<string> { 
            "ambulancia zubnÃ©ho lekÃ¡rstva", "VÅ¡eobecnÃ¡ ambulancia pre deti", "PoÅ¡ty", "restaurant" , "playground", "Vybehy_psy",
            "chemist", "parcel_locker", "MHD", "fitness", "bar", "pub", "fast_food,", "MS", "fitness", "pharmacy",
            "convenience", "cafe", "VÅ¡eobecnÃ¡ ambulancia pre dospelÃ½ch",
            };
            string file = @"..\..\..\..\..\Datasets\POIs_location_catchments.csv";
            float minX = int.MaxValue;
            float maxX = int.MinValue;
            float minY = int.MaxValue;
            float maxY = int.MinValue;

            List<ServiceLocation> services = new List<ServiceLocation>();
            Dictionary<ServiceLocation, float[]> serviceGPS = new Dictionary<ServiceLocation, float[]>();

            using (StreamReader sr = new StreamReader(file))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null)
                {
                    string[] line = currentLine.Split(',');
                    if (types.Contains(line[2]))
                    {
                        if (float.Parse(line[4]) < minX) minX = float.Parse(line[4]);
                        if (float.Parse(line[4]) > maxX) maxX = float.Parse(line[4]);
                        if (float.Parse(line[5]) < minY) minY = float.Parse(line[5]);
                        if (float.Parse(line[5]) > maxY) maxY = float.Parse(line[5]);


                        ServiceLocation service = new ServiceLocation();
                        service.Definition = new ServiceDefinition { Name = line[1], Type = line[2] };
                        serviceGPS.Add(service, new float[] { float.Parse(line[4]), float.Parse(line[5]) });
                        services.Add(service);
                    }
                }
            }

            float unitX = (maxX - minX) /  100; //map length
            float unitY = (maxY - minY) / 100; //map length

            foreach (var s in services)
            {
                s.X = Convert.ToInt32(((serviceGPS[s][0] - minX) / unitX));
                s.Y = Convert.ToInt32((serviceGPS[s][1] - minY) / unitY);
            }

            return services;
        }
    }
}
