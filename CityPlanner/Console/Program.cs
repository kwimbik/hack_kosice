using System;

namespace CityConsole
{
    internal class Program
    {
        const int POP_SIZE = 100;
        const int GENS = 100;
        const int IND_SIZE = 4;
        const int MAP_WIDTH = 100;
        const int MAP_HEIGHT = 100;
        static Random rand = new(666);

        static void Main(string[] args)
        {
            RunEvo();
        }

        static void RunEvo()
        {
            Individual[] population = InitPopulation(POP_SIZE, IND_SIZE);
            for (int g = 0; g < GENS; g++)
            {
                double[] fitnesses = population.Select(i => Fitness(i)).ToArray();
                
                Console.WriteLine($"{g}> Fitness {fitnesses.Average()}");
                int maxIndex = Array.IndexOf(fitnesses, fitnesses.Max());
                Console.WriteLine($"{g}> Cords {string.Join(';',population[maxIndex].Services)}");

                Individual[] mating_pool = Selection(population, fitnesses);
                Individual[] offspring = Mate(mating_pool, 0.7);
                offspring = Mutate(offspring, 0.2, 0.1);
                population = offspring;
            }
        }

        static double Distance(Cords c1, Cords c2)
        {
            return Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y);
        }

        static double Fitness(Individual individual)
        {
            double invFitness = 0;
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    double minDist = double.MaxValue;
                    for (int i = 0; i < individual.Services.Length; i++)
                    {
                        double dist = Distance(new Cords(x, y), individual.Services[i]);
                        if (dist < minDist) minDist = dist;
                    }
                    invFitness += minDist;
                }
            }
            return 1/invFitness;
        }

        static Individual[] InitPopulation(int popSize, int indSize)
        {
            Individual[] population = new Individual[popSize];
            for (int i = 0; i < popSize; i++)
            {
                Individual ind = new();
                ind.Services = new Cords[indSize];
                for (int j = 0; j < indSize; j++)
                {
                    ind.Services[j] = new Cords() { X = rand.Next(MAP_WIDTH), Y = rand.Next(MAP_HEIGHT) };
                }
                population[i] = ind;

            }
            return population;
        }

        static Individual[] Selection(Individual[] population, double[] fitnesses)
        {
            // Tournament selection
            Individual[] newPopulation = new Individual[population.Length];
            for (int i = 0; i < population.Length; i++)
            {
                int p1I = rand.Next(population.Length);
                int p2I = rand.Next(population.Length);
                if (fitnesses[p1I] > fitnesses[p2I] && rand.NextDouble() < 0.8)
                {
                    newPopulation[i] = population[p1I].Clone();
                }
                else
                {
                    newPopulation[i] = population[p2I].Clone();
                }
            }
            return newPopulation;
        }

        static Individual[] Mate(Individual[] population, double cxProb)
        {
            // Uniform xover
            Individual[] newPopulation = new Individual[population.Length];
            for (int i = 0; i < population.Length - 1; i += 2)
            {
                Individual p1 = population[i];
                Individual p2 = population[i + 1];

                int xoverIndex = rand.Next(p1.Services.Length);

                Individual o1 = p1.Clone();
                Array.Copy(p1.Services, 0, o1.Services, 0, xoverIndex);
                Array.Copy(p2.Services, xoverIndex, o1.Services, xoverIndex, p2.Services.Length - xoverIndex);

                Individual o2 = p2.Clone();
                Array.Copy(p1.Services, 0, o2.Services, 0, xoverIndex);
                Array.Copy(p2.Services, xoverIndex, o2.Services, xoverIndex, p2.Services.Length - xoverIndex);

                newPopulation[i] = o1;
                newPopulation[i + 1] = o2;
            }
            return newPopulation;
        }

        static Individual[] Mutate(Individual[] population, double mutProbSmall, double mutProbLarge)
        {
            for (int i = 0; i < population.Length; i++)
            {
                if (rand.NextDouble() < mutProbSmall)
                {
                    int index = rand.Next(population[i].Services.Length);
                    if (rand.NextDouble() < 0.5)
                    {
                        population[i].Services[index].X += rand.NextDouble() < 0.5 ? 1 : -1;
                    }
                    else
                    {
                        population[i].Services[index].Y += rand.NextDouble() < 0.5 ? 1 : -1;
                    }
                }
                else if (rand.NextDouble() < mutProbSmall)
                {
                    int index = rand.Next(population[i].Services.Length);
                    if (rand.NextDouble() < 0.5)
                    {
                        population[i].Services[index].X += rand.NextDouble() < 0.5 ? 5 : -5;
                    }
                    else
                    {
                        population[i].Services[index].Y += rand.NextDouble() < 0.5 ? 5 : -5;
                    }

                }
            }
            return population;
        }
    }

    class Individual
    {
        public Cords[] Services { get; set; }

        public Individual Clone()
        {
            Individual copy = new() { Services = new Cords[Services.Length] };
            Array.Copy(Services, copy.Services, Services.Length);
            return copy;
        }
    }

    struct Cords
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Cords(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}