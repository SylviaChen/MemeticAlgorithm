using System;
using System.Collections.Generic;
using System.Linq;

namespace MemeticAlgorithm
{
    public class MaterializedView
    {
        private readonly View _view;
        private readonly Chromosome _chromosome;


        public MaterializedView(View view, Chromosome chromosome)
        {
            _view = view;
            _chromosome = chromosome;
        }
        public Chromosome MemeticAlgorithmViewSelection(int generations, float tournamentChance, float crossOverProbability, float mutationProbability, int topViewCount, int firstPoulationCount)
        {
            List<View> lattice = _view.CreateLattice();
            List<Chromosome> population = _chromosome.PopulateChromosome(lattice, topViewCount, firstPoulationCount);

            for (int i = 0; i < generations; i++)
            {
                population = SetTvecForEachChromosome(lattice, population);

                List<Chromosome> offSpringPopulation = TournamentSelection(population, tournamentChance, 10);

                List<Chromosome> crossOver = CrossOver(offSpringPopulation, crossOverProbability, 10);
                List<Chromosome> mutation = Mutation(lattice, crossOver, mutationProbability, 10, topViewCount);

                population.Clear();
                population = mutation;
            }

            return population.First(p => p.Tvec == population.Max(c => c.Tvec));
        }

        private Chromosome IterativeImprovement(List<View> lattice, Chromosome chromosome, int topViewCount)
        {
            Random random = new Random();

            View selectedView = lattice[random.Next(lattice.Count - 1)];

            Chromosome newChromosome = new Chromosome();
            Chromosome tempChromosome = new Chromosome();

            tempChromosome = chromosome;
            newChromosome = tempChromosome;
            newChromosome.Views.Remove(chromosome.Views[random.Next(1, topViewCount - 1)]);

            newChromosome.Views.Add(selectedView);

            if (IsValidChromosome(chromosome.Views))
            {
                newChromosome.Tvec= CalculateFitnessFunction(lattice, chromosome.Views);
                if (newChromosome.Tvec > tempChromosome.Tvec)
                    tempChromosome = newChromosome;
            }
                 
            return null;
        }

        private double CalculateFitnessFunction(List<View> lattice, List<View> chromosome)
        {
            List<View> tempLattice = lattice.ToList();
            double sizeView = 0;
            double sizeSmaView = 0;

            foreach (var gene in chromosome)
            {
                sizeView += gene.Size;
                tempLattice.Remove(gene);
            }

            foreach (var view in tempLattice)
            {
                if (chromosome.Any(t => t.Level < view.Level))
                    sizeSmaView += chromosome.Where(c => c.Level < view.Level && c.Dimensions.Intersect(view.Dimensions).Any()).Min(c => c.Size);
            }

            return sizeView + sizeSmaView;
        }

        private List<Chromosome> TournamentSelection(List<Chromosome> population, float chance, int tournamentIteration)
        {
            Random random = new Random();
            List<Chromosome> result = new List<Chromosome>();

            for (int i = 0; i < tournamentIteration; i++)
            {
                Chromosome firstChromosome = population[random.Next(population.Count)];
                Chromosome secondChromosome = population[random.Next(population.Count)];

                if (random.NextDouble() < chance)
                    result.Add(firstChromosome.Tvec >= secondChromosome.Tvec ? firstChromosome : secondChromosome);
                else
                    result.Add(firstChromosome.Tvec < secondChromosome.Tvec ? firstChromosome : secondChromosome);
            }

            return result;
        }

        private List<Chromosome> CrossOver(List<Chromosome> population, float probability,
            int iteration)
        {
            Random random = new Random();
            double crossOverPoint = Math.Floor((double)(population[0].Views.Count - 1) / 2);
            List<Chromosome> result = new List<Chromosome>();

            for (int i = 0; i < iteration; i++)
            {
                Chromosome father = population[random.Next(population.Count)];
                Chromosome mother = population[random.Next(population.Count)];

                if (random.NextDouble() > probability)
                {
                    List<View> firstChildViews = new List<View> { father.Views[0] };
                    List<View> secondChildViews = new List<View> { father.Views[0] };

                    for (int j = 1; j <= crossOverPoint; j++)
                    {
                        firstChildViews.Add(father.Views[j]);
                        secondChildViews.Add(mother.Views[j]);
                    }

                    for (int j = (int)crossOverPoint + 1; j < population[0].Views.Count; j++)
                    {
                        firstChildViews.Add(mother.Views[j]);
                        secondChildViews.Add(father.Views[j]);
                    }

                    if (IsValidChromosome(firstChildViews))
                        result.Add(new Chromosome
                        {
                            Views = firstChildViews,
                            Tvec = 0
                        });

                    if (IsValidChromosome(secondChildViews))
                        result.Add(new Chromosome
                        {
                            Views = secondChildViews,
                            Tvec = 0
                        });
                }
                else
                {
                    result.AddRange(new[] { father, mother });
                }
            }

            return result;
        }

        private List<Chromosome> Mutation(List<View> lattice, List<Chromosome> population, float probability,
            int iteration, int topViewCount)
        {
            Random random = new Random();
            List<Chromosome> result = new List<Chromosome>();

            for (int i = 0; i < iteration; i++)
            {
                Chromosome chromosome = population[random.Next(population.Count)];

                if (random.NextDouble() <= probability)
                {
                    View selectedView = lattice[random.Next(lattice.Count - 1)];

                    chromosome.Views.Remove(chromosome.Views[random.Next(1, topViewCount - 1)]);

                    chromosome.Views.Add(selectedView);

                    if (IsValidChromosome(chromosome.Views))
                        result.Add(new Chromosome
                        {
                            Views = chromosome.Views,
                            Tvec = 0
                        });
                }
                else
                {
                    result.AddRange(new[] { chromosome });
                }
            }

            return result;
        }

        private bool IsValidChromosome(List<View> chromosome)
        {
            return !chromosome.GroupBy(a => a.Label).Any(a => a.Count() > 1);
        }

        private List<Chromosome> SetTvecForEachChromosome(List<View> lattice, List<Chromosome> population)
        {
            foreach (var chromosome in population)
            {
                if ((int)chromosome.Tvec == 0)
                    chromosome.Tvec = CalculateFitnessFunction(lattice, chromosome.Views);
            }

            return population;
        }
    }
}
