using System.Collections.Generic;

namespace MemeticAlgorithm
{
    public class View
    {
        public int Level { get; set; }

        public string Label { get; set; }

        public List<string> Dimensions { get; set; }

        public int Frequency { get; set; }

        public int Size { get; set; }

        public List<View> CreateLattice()
        {
            return new List<View>
            {
                new View
                {
                    Level = 1,
                    Label = "ABC",
                    Dimensions =new List<string>(new [] {"A","B","C"}) ,
                    Frequency = 5000,
                    Size = 50
                },
                new View
                {
                    Level = 2,
                    Label = "AB",
                    Dimensions =new List<string>(new [] {"A","B"}),
                    Frequency = 2888,
                    Size = 38
                },
                new View
                {
                    Level = 2,
                    Label = "AC",
                    Dimensions =new List<string>(new [] {"A","C"}),
                    Frequency = 1995,
                    Size = 35
                },
                new View
                {
                    Level = 2,
                    Label = "BC",
                    Dimensions =new List<string>(new [] {"B","C"}),
                    Frequency = 1568,
                    Size = 28
                },
                new View
                {
                    Level = 3,
                    Label = "A",
                    Dimensions = new List<string>(new []{"A"}),
                    Frequency = 648,
                    Size = 18
                },
                new View
                {
                    Level = 3,
                    Label = "B",
                    Dimensions = new List<string>(new []{"B"}),
                    Frequency = 392,
                    Size = 14
                },
                new View
                {
                    Level = 3,
                    Label = "C",
                    Dimensions =new List<string>(new [] {"C"}),
                    Frequency = 512,
                    Size = 16
                },
                new View
                {
                    Level = 4,
                    Label = "NONE",
                    Dimensions =new List<string>(new [] {"A","B","C"}),
                    Frequency = 0,
                    Size = 1
                }
            };
        }
    }
}
