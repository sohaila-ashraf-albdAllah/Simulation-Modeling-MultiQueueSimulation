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

            last_finish_time = 0;
        }

        public int ID { get; set; }
        public decimal IdleProbability { get; set; }
        public decimal AverageServiceTime { get; set; } 
        public decimal Utilization { get; set; }
        public int last_finish_time { get; set; }

        public List<TimeDistribution> TimeDistribution;

        //optional if needed use them
        public int FinishTime { get; set; }
        public int totalruntimeSimulation { get; set; }

        public int total_num_of_Customers , TotalIdleTime;
        public void calculate_total_runtime_simulation(SimulationSystem Simulation_sys)
        {
            for (int i = 0; i < Simulation_sys.SimulationTable.Count; i++)
            {

                if (Simulation_sys.SimulationTable[i].ServerIndex == ID)
                {
                    totalruntimeSimulation += Simulation_sys.SimulationTable[i].ServiceTime;
                    total_num_of_Customers++;
                }
            }

        }
        public void calculate_IdleProb(SimulationSystem Simulation_sys)
        {
            TotalIdleTime = Simulation_sys.TotalSimulationTime - totalruntimeSimulation;
            if (Simulation_sys.TotalSimulationTime == 0)
            {
                IdleProbability = 0;
            }
            else
            {
                IdleProbability = (decimal)TotalIdleTime / Simulation_sys.TotalSimulationTime;
            }
        }
        public void calculate_average_service_time()
        {
            if (total_num_of_Customers == 0)
            {
                AverageServiceTime = 0;
            }
            else
            {
                AverageServiceTime = (decimal)totalruntimeSimulation / total_num_of_Customers;
            }
        }
       
        public void calculate_Utilization(SimulationSystem Simulation_sys)
        {
            if (Simulation_sys.TotalSimulationTime == 0)
            {
                Utilization = 0;
            }
            else
            {
                Utilization = (decimal)totalruntimeSimulation / Simulation_sys.TotalSimulationTime;
            }

        }
    }
}
