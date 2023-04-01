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
    public static class ServiceParser
    {

        public static void parseServices(Map map)
        {
            List<string> types = new List<string> { 
            "ambulancia zubnÃ©ho lekÃ¡rstva", "VÅ¡eobecnÃ¡ ambulancia pre deti", "PoÅ¡ty", "restaurant" , "playground", "Vybehy_psy",
            "chemist", "parcel_locker", "MHD", "fitness", "bar", "pub", "fast_food,", "MS", "fitness", "pharmacy",
            "convenience", "cafe", "VÅ¡eobecnÃ¡ ambulancia pre dospelÃ½ch",
            };
            string file = @"..\..\..\..\..\Datasets\POIs_location_catchments.csv";
          
            List<ServiceLocation> services = new List<ServiceLocation>();

            using (StreamReader sr = new StreamReader(file))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null)
                {
                    string[] line = currentLine.Split(',');
                    if (types.Contains(line[2]))
                    {
                        ServiceLocation service = new ServiceLocation();
                        service.Definition = new ServiceDefinition { Name = line[1], Type = line[2] };
                        service.X = Convert.ToInt32(((float.Parse(line[4]) - map.Min_X) / map.Unit_X));
                        service.Y = Convert.ToInt32(((float.Parse(line[5]) - map.Min_Y) / map.Unit_Y));
                        services.Add(service);
                    }
                }
            }
            map.services = services;
        }
    }
}
