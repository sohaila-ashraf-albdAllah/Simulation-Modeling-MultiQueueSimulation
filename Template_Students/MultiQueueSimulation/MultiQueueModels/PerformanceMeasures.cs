using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class PerformanceMeasures
    {
        public decimal AverageWaitingTime { get; set; }
        public int MaxQueueLength { get; set; }
        public decimal WaitingProbability { get; set; }
        public struct PAIR
        {
            public int first;
            public int second;
        }
        public void MaxQLength(SimulationSystem S_sys)/////////////
        {
            List<int> PartialSum = new List<int>();
            List<PAIR> Ranges = new List<PAIR>();

            int mx = -100000;
            for (int i = 0; i < S_sys.SimulationTable.Count; i++)
            {
                int cur = S_sys.SimulationTable[i].EndTime;
                if (mx < cur)
                {
                    mx = cur;
                }
                PAIR tmp;
                tmp.first = S_sys.SimulationTable[i].ArrivalTime;
                tmp.second = S_sys.SimulationTable[i].StartTime;
                if (tmp.first != tmp.second) Ranges.Add(tmp);
            }
            for (int i = 0; i <= mx; i++)
            {
                PartialSum.Add(0);
            }
            for (int i = 0; i < Ranges.Count; i++)
            {
                PartialSum[Ranges[i].first]++;
                PartialSum[Ranges[i].second + 1]--;
            }
            MaxQueueLength = -100000;
            for (int i = 1; i < PartialSum.Count; i++)
            {
                PartialSum[i] += PartialSum[i - 1];
                if (MaxQueueLength < PartialSum[i])
                {
                    MaxQueueLength = PartialSum[i];
                }
            }
        }
        public void calculate_system_Performance(SimulationSystem S_sys)//d
        {

            S_sys.waitingQu = S_sys.waitingCustomersInQu();
            if (S_sys.SimulationTable.Count == 0)
            {
                AverageWaitingTime = 0;
                WaitingProbability = 0;
            }
            else
            {
                AverageWaitingTime = (decimal)S_sys.waitingQu.ElementAt(0).Value / S_sys.SimulationTable.Count;
                WaitingProbability = (decimal)S_sys.waitingQu.ElementAt(0).Key / S_sys.SimulationTable.Count;
            }
            MaxQLength(S_sys);
        }
        public void calculate_server_Performance(SimulationSystem S_sys)//d
        {
            S_sys.calculate_TotalSimulationTime();
            for (int i = 0; i < S_sys.Servers.Count; i++)
            {
                S_sys.Servers[i].calculate_workingtimeOfServer(S_sys);
                S_sys.Servers[i].calculate_Utilization(S_sys);
                S_sys.Servers[i].calculate_IdleProb(S_sys);
                S_sys.Servers[i].calculate_average_service_time();
            }

        }
    }
}