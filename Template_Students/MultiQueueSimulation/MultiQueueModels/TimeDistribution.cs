using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class TimeDistribution
    {
        public int Time { get; set; }
        public decimal Probability { get; set; }
        public decimal CummProbability { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }
        public decimal calculate_CummProbability(decimal last_brop)//d
        {
            CummProbability = this.Probability + last_brop;
            return CummProbability;
        }
        public void calculate_Ranges(int min_range)//d
        {
            MaxRange = (int)(CummProbability * 100);
            MinRange = min_range;
        }

    }
}
