using CityPlanner.Models;
using CityPlanner;
using System.Data;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Map map = new Map();
            DemographicUnit u1 = new DemographicUnit();
            DemographicUnit u2 = new DemographicUnit();
            DemographicUnit u3 = new DemographicUnit();
            DemographicUnit u4 = new DemographicUnit();
            DemographicUnit u5 = new DemographicUnit();
            DemographicUnit u6 = new DemographicUnit();
            map.Matrix = new DemographicUnit[,] { { u1, u2, u3 }, { u4, u5, u6 } };

            ServiceDefinition definition = new ServiceDefinition();
            ServiceLocation l1 = new ServiceLocation();
            ServiceLocation l2 = new ServiceLocation();

            float[,] stats = Stats.getServiceStats(map, new List<ServiceLocation> { l1, l2 });
            Assert.IsTrue(stats[0, 0] <= 2 && stats[0, 0] >= 0);
            Assert.AreEqual(3, stats.GetLength(0));
            Assert.AreEqual(2, stats.GetLength(1));
        }
    }
}