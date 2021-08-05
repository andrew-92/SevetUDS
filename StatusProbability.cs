using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeventUDS_2_2
{
    public class StatusProbability
    {
        public string Name { get; set; }
        public int? OSValue { get; set; }
        public bool Available { get; set; } = true;
        public double Probability { get; set; }
        public double Counter { get; set; }
        private int _amountOfRents;
        public int AmountOfRents {
            get { return _amountOfRents; }
            set
            {
                _amountOfRents = value;
                Counter = Probability * value;
            } 
        }

        public StatusProbability() { }

        public StatusProbability(string name, int value, double probability, int amountOfRents)
        {
            Name = name;
            OSValue = value;
            Probability = probability;
            AmountOfRents = amountOfRents;
            Counter = amountOfRents * probability;
        }
    }
}
