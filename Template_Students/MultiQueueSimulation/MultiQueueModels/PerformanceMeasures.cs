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
        public void MaxQLength(SimulationSystem S_sys)//d
        {
            int temp;
            for (int i = S_sys.Servers.Count; i < S_sys.SimulationTable.Count; i++)
            {
                temp = 0;
                for (int j = i +1; j < S_sys.SimulationTable.Count; j++)
                {
                    if(S_sys.SimulationTable[i].TimeInQueue > 0)
                    {
                        if (S_sys.SimulationTable[j].ArrivalTime < S_sys.SimulationTable[i].StartTime 
                            && S_sys.SimulationTable[j].TimeInQueue > 0)
                        {
                            temp++;
                        }
                        else
                        {
                            break;
                        }
                    }             
                }
                if (S_sys.SimulationTable[i].TimeInQueue > 0)
                {
                    temp++;
                }
                if (temp > MaxQueueLength)
                {
                    MaxQueueLength = temp;
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