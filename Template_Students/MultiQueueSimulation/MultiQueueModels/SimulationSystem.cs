using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiQueueModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            this.Servers = new List<Server>();
            this.InterarrivalDistribution = new List<TimeDistribution>();
            this.PerformanceMeasures = new PerformanceMeasures();
            this.SimulationTable = new List<SimulationCase>();

            this.availableServers = new List<Server>();

        }

        ///////////// INPUTS ///////////// 
        public int NumberOfServers { get; set; }
        public int StoppingNumber { get; set; }
        public List<Server> Servers { get; set; }
        public List<TimeDistribution> InterarrivalDistribution { get; set; }
        public Enums.StoppingCriteria StoppingCriteria { get; set; }
        public Enums.SelectionMethod SelectionMethod { get; set; }

        public int CustomersNumber { get; set; }
        List<Server> availableServers;
        public int TotalSimulationTime;

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }

        public void calculate_TotalSimulationTime()
        {
            int totalTime = -1;
            for (int i = 0; i < SimulationTable.Count; i++)
            {
                int temp = SimulationTable[i].EndTime;
                if (temp > totalTime) 
                { 
                    totalTime = temp; 
                }

            }
            TotalSimulationTime = totalTime;
        }
        public void Generate_Distribution_table(List<TimeDistribution> TDT)
        {
            TimeDistribution lastRow = new TimeDistribution();
            lastRow.CummProbability = 0;
            lastRow.MaxRange = 0;
            for (int i = 0; i < TDT.Count; i++)
            {
                TDT[i].calculate_CummProbability(lastRow.CummProbability);
                TDT[i].calculate_Ranges(lastRow.MaxRange + 1);
                lastRow = TDT[i];
            }
        }
        public void createServerTable(List<Server> numOfServers)
        {
            for (int i = 0; i < numOfServers.Count; i++)
            {
                Generate_Distribution_table(numOfServers[i].TimeDistribution);
            }
        }
        public int checkRange(int Random)
        {
            int T = 0;
            for (int i = 0; i < InterarrivalDistribution.Count; i++)
            {
                if (Random >= InterarrivalDistribution[i].MinRange && Random <= InterarrivalDistribution[i].MaxRange)
                {
                    T = InterarrivalDistribution[i].Time;
                    return T;
                }
            }
            return -1;
        }
        public int First_Server_Finished()
        {
            int min = 1000000000, result = 0;
            for (int i = 0; i < Servers.Count; i++)
            {
                if (Servers[i].last_finish_time < min)
                {
                    result = i;
                    min = Servers[i].last_finish_time;
                }
            }
            return result;
        }
        public int idleServersList(int arrivalTimeOfCustomer)
        {
            availableServers = new List<Server>();
            for (int i = 0; i < Servers.Count; i++)
            {
                if (arrivalTimeOfCustomer >= Servers[i].last_finish_time)
                {
                    availableServers.Add(Servers[i]);
                    return 1;
                }
            }
            return -1;
        }

        public void Simulate()
        {
            int existingCustomerNumber = 1;
            int ServerIndex = 0;
            SimulationCase lastCustomer = new SimulationCase();
            Random interArrivalRandom = new Random();
            Random ServiceRandom = new Random();
            Random ServerRandom = new Random();
            lastCustomer.ArrivalTime = 0;
            lastCustomer.InterArrival = 0;
            lastCustomer.EndTime = 0;
            while (true)
            {
                //Our Main code!
                if (StoppingCriteria == (MultiQueueModels.Enums.StoppingCriteria.NumberOfCustomers ) && existingCustomerNumber > StoppingNumber)
                {
                    break;
                }
                else if(StoppingCriteria == (MultiQueueModels.Enums.StoppingCriteria.SimulationEndTime) && TotalSimulationTime > StoppingNumber)
                {
                    break;
                }
                SimulationCase newCustomer = new SimulationCase();
                newCustomer.CustomerNumber = existingCustomerNumber;
                newCustomer.RandomInterArrival = interArrivalRandom.Next(1, 101);
                newCustomer.InterArrival = checkRange(newCustomer.RandomInterArrival);

                if (existingCustomerNumber == 1)
                {
                    newCustomer.ArrivalTime = lastCustomer.ArrivalTime;
                }
                else
                {
                    newCustomer.ArrivalTime = lastCustomer.ArrivalTime + newCustomer.InterArrival;
                }
                //if there is a avalid servers
                if (idleServersList(newCustomer.ArrivalTime) == 1)
                {
                    newCustomer.TimeInQueue = 0;
                    if (SelectionMethod == Enums.SelectionMethod.HighestPriority)
                    {
                        newCustomer.AssignedServer = availableServers[0];
                        ServerIndex = availableServers[0].ID;
                        availableServers.RemoveAt(0);
                    }
                    else if (SelectionMethod == Enums.SelectionMethod.Random)
                    {
                        int maxRang = availableServers.Count;
                        int RandomServer = ServerRandom.Next(0, maxRang);
                        newCustomer.AssignedServer = availableServers[RandomServer];
                        ServerIndex = availableServers[RandomServer].ID;
                    }
                    newCustomer.indexOfServers = ServerIndex;
                    
                    if (newCustomer.AssignedServer.last_finish_time > newCustomer.ArrivalTime)
                    {
                        newCustomer.StartTime = newCustomer.AssignedServer.last_finish_time;
                    }
                    else
                    {
                        newCustomer.StartTime = newCustomer.ArrivalTime;

                    }
                    newCustomer.RandomService = ServiceRandom.Next(1, 101);
                    //////////???????????????
                   /* if (newCustomer.RandomService == 0)
                    {
                        newCustomer.RandomService = 1;
                    }*/
                    newCustomer.ServiceTime = checkRange(newCustomer.RandomService);
                    newCustomer.EndTime = newCustomer.StartTime + newCustomer.ServiceTime;
                    Servers[ServerIndex].last_finish_time = newCustomer.EndTime;
                }
                else
                {
                    //Time in queue
                    ServerIndex = First_Server_Finished();
                    newCustomer.AssignedServer = Servers[ServerIndex];
                    newCustomer.StartTime = newCustomer.AssignedServer.last_finish_time;
                    newCustomer.TimeInQueue = newCustomer.StartTime - newCustomer.ArrivalTime;
                    newCustomer.RandomService = ServiceRandom.Next(1, 101);
                    newCustomer.ServiceTime = checkRange(newCustomer.RandomService);
                    newCustomer.EndTime = newCustomer.StartTime + newCustomer.ServiceTime;
                    newCustomer.indexOfServers = ServerIndex + 1;
                    Servers[ServerIndex].last_finish_time = newCustomer.EndTime;
                }
                SimulationTable.Add(newCustomer);
                lastCustomer = newCustomer;
                existingCustomerNumber++;
            }

            //Don't forget PerformanceMeasures
            PerformanceMeasures.CalcServerPerformance(this);
            PerformanceMeasures.CalcSysPerformance(this);
            //System.PerformanceMeasures
            MessageBox.Show("End of Simulation");
        }
    }
}
