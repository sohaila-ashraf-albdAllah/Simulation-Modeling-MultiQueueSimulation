using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class Server
    {
        public Server()
        {
            this.TimeDistribution = new List<TimeDistribution>();
            AverageServiceTime = 0;
            Utilization = 0;
            last_finish_time = 0;
        }
        public int ID { get; set; }
        public decimal IdleProbability { get; set; }
        public decimal AverageServiceTime { get; set; }
        public decimal Utilization { get; set; }
        public int last_finish_time { get; set; }//note

        public List<TimeDistribution> TimeDistribution;

        //optional if needed use them
        public int FinishTime { get; set; }
       // public int totalruntimeSimulation { get; set; }
        public int total_num_of_Customers { get; set; }
        public int TotalIdleTime { get; set; }//pls
        public int TotalWorkingTime { get; set; }

        public void calculate_workingtimeOfServer(SimulationSystem S_sys)//d
        {
            for (int i = 0; i < S_sys.SimulationTable.Count; i++)
            {
                if (S_sys.SimulationTable[i].ServerIndex == ID)
                {
                    TotalWorkingTime = TotalWorkingTime + S_sys.SimulationTable[i].ServiceTime;
                    total_num_of_Customers += 1;
                }
            }
        }

        public void calculate_IdleProb(SimulationSystem S_sys)//d
        {
            if (S_sys.TotalSimulationTime == 0)
            {
                IdleProbability = 0;
            }
            else
            {
                TotalIdleTime = S_sys.TotalSimulationTime - TotalWorkingTime;
                IdleProbability = (decimal)(TotalIdleTime) / S_sys.TotalSimulationTime;
            }
        }
        public void calculate_Utilization(SimulationSystem Simulation_sys)//d
        {
            if (Simulation_sys.TotalSimulationTime == 0)
            {
                Utilization = 0;
            }
            else
            {
                Utilization = (decimal)TotalWorkingTime / Simulation_sys.TotalSimulationTime;
            }

        }
        public void calculate_average_service_time()//d
        {
            if (total_num_of_Customers == 0)
            {
                AverageServiceTime = 0;
            }
            else
            {
                AverageServiceTime = (decimal)TotalWorkingTime / total_num_of_Customers;
            }
        }
    }
}
