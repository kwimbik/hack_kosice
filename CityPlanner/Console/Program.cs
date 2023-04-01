using System;

namespace Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, Dictionary<int, bool>> isNeighbors = new();

            int mapSize = 200;
            int mapArea = mapSize * mapSize;

            for (int i = 0; i < mapArea; i++)
            {
                isNeighbors[i] = new();
                for (int j = 0; j < mapArea; j++)
                {
                    isNeighbors[i][j] = i < j;
                }
            }

            System.Console.WriteLine("Done");
        }
    }
}