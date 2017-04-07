using System;
using System.Collections.Generic;

namespace MemeticAlgorithm
{
    public class Chromosome
    {
        public List<View> Views { get; set; }
        public double Tvec { get; set; }

        public List<Chromosome> PopulateChromosome(List<View> lattice, int topViewCount, int populationCount)
        {
            List<Chromosome> chromosomes = new List<Chromosome>();

            Random random = new Random();
            for (int i = 0; i < populationCount; i++)
            {
                List<View> chromosome = new List<View> { lattice[0] };

                while (chromosome.Count < topViewCount + 1)
                {
                    int selectedViewIndex = random.Next(lattice.Count - 1);

                    if (!chromosome.Contains(lattice[selectedViewIndex]))
                    {
                        chromosome.Add(lattice[selectedViewIndex]);
                    }
                }

                chromosomes.Add(new Chromosome
                {
                    Views = chromosome,
                    Tvec = 0
                });
            }
            return chromosomes;
        }
    }
}
