﻿using CityPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner
{
    internal class Evolution
    {
        public delegate void GenerationEventHandler(int generation, Individual[] population, double[] fitnesses);

        private Random _rand = new();

        public event GenerationEventHandler GenerationEvent;

        private Map _map;

        public void Run(Map map, int popSize, int indSize, int generations)
        {
            _map = map;

            Individual[] population = InitPopulation(popSize, indSize);
            for (int g = 0; g < generations; g++)
            {
                double[] fitnesses = population.Select(i => Fitness(i)).ToArray();

                //Console.WriteLine($"{g}> Fitness {fitnesses.Average()}");
                //int maxIndex = Array.IndexOf(fitnesses, fitnesses.Max());
                //Console.WriteLine($"{g}> Cords {string.Join(';', population[maxIndex].Services)}");

                if (GenerationEvent is not null) GenerationEvent.Invoke(g, population, fitnesses);

                Individual[] mating_pool = Selection(population, fitnesses);
                Individual[] offspring = Mate(mating_pool, 0.7);
                offspring = Mutate(offspring, 0.2, 0.1);
                population = offspring;
            }
        }

        private double Distance(Cords c1, Cords c2)
        {
            return (Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y)) * _map.Matrix[(int)((c1.X)), (int)((c1.Y))].Population;
        }

        private double Fitness(Individual individual)
        {
            double invFitness = 0;
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    double minDist = double.MaxValue;
                    for (int i = 0; i < individual.Services.Length; i++)
                    {
                        double dist = Distance(new Cords(x, y), individual.Services[i]) ;
                        if (dist < minDist) minDist = dist;
                    }
                    invFitness += minDist;
                }
            }
            return 1 / invFitness;
        }

        private Individual[] InitPopulation(int popSize, int indSize)
        {
            Individual[] population = new Individual[popSize];
            for (int i = 0; i < popSize; i++)
            {
                Individual ind = new();
                ind.Services = new Cords[indSize];
                for (int j = 0; j < indSize; j++)
                {
                    ind.Services[j] = new Cords() { X = _rand.Next(_map.Width), Y = _rand.Next(_map.Height) };
                }
                population[i] = ind;

            }
            return population;
        }

        private Individual[] Selection(Individual[] population, double[] fitnesses)
        {
            // Tournament selection
            Individual[] newPopulation = new Individual[population.Length];
            for (int i = 0; i < population.Length; i++)
            {
                int p1I = _rand.Next(population.Length);
                int p2I = _rand.Next(population.Length);
                if (fitnesses[p1I] > fitnesses[p2I] && _rand.NextDouble() < 0.8)
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

        private Individual[] Mate(Individual[] population, double cxProb)
        {
            // Uniform xover
            Individual[] newPopulation = new Individual[population.Length];
            for (int i = 0; i < population.Length - 1; i += 2)
            {
                Individual p1 = population[i];
                Individual p2 = population[i + 1];

                int xoverIndex = _rand.Next(p1.Services.Length);

                Individual o1 = p1.Clone();
                Array.Copy(p1.Services, 0, o1.Services, 0, xoverIndex);
                Array.Copy(p2.Services, xoverIndex, o1.Services, xoverIndex, p2.Services.Length - xoverIndex);

                Individual o2 = p2.Clone();
                Array.Copy(p2.Services, 0, o2.Services, 0, xoverIndex);
                Array.Copy(p1.Services, xoverIndex, o2.Services, xoverIndex, p1.Services.Length - xoverIndex);

                newPopulation[i] = o1;
                newPopulation[i + 1] = o2;
            }
            return newPopulation;
        }

        private Individual[] Mutate(Individual[] population, double mutProbSmall, double mutProbLarge)
        {
            for (int i = 0; i < population.Length; i++)
            {
                if (_rand.NextDouble() < mutProbSmall)
                {
                    int index = _rand.Next(population[i].Services.Length);
                    double val = _rand.NextDouble();
                    if (val < 0.3)
                    {
                        population[i].Services[index].X += _rand.NextDouble() < 0.5 ? 1 : -1;
                    }
                    else if (val < 0.6)
                    {
                        population[i].Services[index].Y += _rand.NextDouble() < 0.5 ? 1 : -1;
                    }
                    else
                    {
                        population[i].Services[index].X += _rand.NextDouble() < 0.5 ? 1 : -1;
                        population[i].Services[index].Y += _rand.NextDouble() < 0.5 ? 1 : -1;
                    }
                }
                else if (_rand.NextDouble() < mutProbLarge)
                {
                    int index = _rand.Next(population[i].Services.Length);
                    double val = _rand.NextDouble();
                    if (val < 0.3)
                    {
                        population[i].Services[index].X += _rand.NextDouble() < 0.5 ? 5 : -5;
                    }
                    else if (val < 0.6)
                    {
                        population[i].Services[index].Y += _rand.NextDouble() < 0.5 ? 5 : -5;
                    }
                    else
                    {
                        population[i].Services[index].X += _rand.NextDouble() < 0.5 ? 5 : -5;
                        population[i].Services[index].Y += _rand.NextDouble() < 0.5 ? 5 : -5;
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

    struct IntCords
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IntCords(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
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
