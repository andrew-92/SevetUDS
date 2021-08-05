using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeventUDS_2_2
{
    public class PaidPobability
    {
        public string Name { get; set; }

        public double Count { get; set; }

        public PaidPobability(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public PaidPobability() { }
    }
}
