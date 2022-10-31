using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MultiQueueModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            this.Servers = new List<Server>();
            this.InterarrivalDistribution = new List<TimeDistribution>();
            this.availableServers = new List<Server>();//pls
            this.PerformanceMeasures = new PerformanceMeasures();
            this.SimulationTable = new List<SimulationCase>();

        }
        ///////////// INPUTS ///////////// 
        public int NumberOfServers { get; set; }
        public int StoppingNumber { get; set; }
        public List<Server> Servers { get; set; }
        public List<TimeDistribution> InterarrivalDistribution { get; set; }
        public Enums.StoppingCriteria StoppingCriteria { get; set; }
        public Enums.SelectionMethod SelectionMethod { get; set; }

        public Dictionary<int, int> waitingQu = new Dictionary<int, int>();//pls
        List<Server> availableServers;//pls
        public int LeastUtilizationServers;//pls
        public int TotalWorkingTimeServer;//pls
        public int TotalSimulationTime;//pls

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }

        /// ///////////////////////////////////////////////////////////
        public Dictionary<int, int> WaitingCustomersInQu()//d
        {
            int count = 0;
            int waitingTime = 0;
            for (int i = 0; i < SimulationTable.Count; i++)
            {
                if (SimulationTable[i].TimeInQueue != 0)
                {
                    count++;
                    waitingTime += SimulationTable[i].TimeInQueue;
                }
            }
            waitingQu.Add(count, waitingTime);
            return waitingQu;
        }
        public void Build_Distribution_table(List<TimeDistribution> TDT)//d
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
        public void BuildServerTable(List<Server> numOfServers)//d
        {
            for (int i = 0; i < numOfServers.Count; i++)
            {
                Build_Distribution_table(numOfServers[i].TimeDistribution);
            }
        }
        public void Calculate_TotalSimulationTime()//d
        {
            int totalTime = -1;
            for (int i = 0; i < SimulationTable.Count; i++)
            {
                if (totalTime < SimulationTable[i].EndTime)
                {
                    totalTime = SimulationTable[i].EndTime;
                }

            }
            TotalSimulationTime = totalTime;
        }
        public int CheckRange(List<TimeDistribution> Table, int Random)//d
        {
            int T = 0;
            for (int i = 0; i < Table.Count; i++)
            {
                if (Random >= Table[i].MinRange && Random <= Table[i].MaxRange)
                {
                    T = Table[i].Time;
                    break;
                }
            }
            return T;
        }
        public int MinFinishTime() //first server finish //d
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
        public int IdleServersList(int arrivalTimeOfCustomer)//d
        {
            availableServers = new List<Server>();
            for (int i = 0; i < Servers.Count; i++)
            {
                if (arrivalTimeOfCustomer >= Servers[i].last_finish_time)
                {
                    availableServers.Add(Servers[i]);
                }
            }
            return availableServers.Count;
        }
        public int CalcLeastUtilizationServer()
        {
            int min = 0 , indx = 0;
            for(int i = 0; i < NumberOfServers; i++)
            {
                for (int j = 0; j < SimulationTable.Count; j++)
                {
                    if (SimulationTable[j].ServerIndex == Servers[i].ID)
                    {
                        TotalWorkingTimeServer += SimulationTable[j].ServiceTime;
                    }
                }
                if(TotalWorkingTimeServer >= min)
                {
                    min = TotalWorkingTimeServer;
                    indx = i;
                }
            }
            return indx;
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
            while (true)
            {
                if (StoppingCriteria == (MultiQueueModels.Enums.StoppingCriteria.NumberOfCustomers) 
                    && existingCustomerNumber > StoppingNumber)
                {
                    break;
                }
                else if (StoppingCriteria == (MultiQueueModels.Enums.StoppingCriteria.SimulationEndTime))
                {
                    Calculate_TotalSimulationTime();
                    if (TotalSimulationTime > StoppingNumber)
                    {
                        break;
                    }
                }
                SimulationCase newCustomer = new SimulationCase();
                newCustomer.CustomerNumber = existingCustomerNumber;
                newCustomer.RandomInterArrival = interArrivalRandom.Next(1, 100);
                newCustomer.InterArrival = CheckRange(InterarrivalDistribution, newCustomer.RandomInterArrival);
                if (existingCustomerNumber == 1)
                {
                    newCustomer.ArrivalTime = lastCustomer.ArrivalTime;
                }
                else
                {
                    newCustomer.ArrivalTime = lastCustomer.ArrivalTime + newCustomer.InterArrival;
                }
                //if there is a avalid servers
                if (IdleServersList(newCustomer.ArrivalTime) > 0)
                {
                    newCustomer.TimeInQueue = 0;
                    if (SelectionMethod == Enums.SelectionMethod.HighestPriority)//d
                    {
                        newCustomer.AssignedServer = availableServers[0];
                        ServerIndex = availableServers[0].ID - 1;
                    }
                    else if (SelectionMethod == Enums.SelectionMethod.Random)
                    {
                        int maxRang = availableServers.Count;
                        int RandomServer = ServerRandom.Next(0, maxRang);
                        newCustomer.AssignedServer = availableServers[RandomServer];
                        ServerIndex = availableServers[RandomServer].ID - 1;
                    }
                    else if (SelectionMethod == Enums.SelectionMethod.LeastUtilization)
                    {
                        LeastUtilizationServers = CalcLeastUtilizationServer();
                        newCustomer.AssignedServer = Servers[LeastUtilizationServers];
                        ServerIndex = Servers[LeastUtilizationServers].ID - 1;
                    }
                    newCustomer.ServerIndex = ServerIndex + 1;
                    if (newCustomer.AssignedServer.last_finish_time > newCustomer.ArrivalTime)
                    {
                        newCustomer.StartTime = newCustomer.AssignedServer.last_finish_time;
                    }
                    else
                    {
                        newCustomer.StartTime = newCustomer.ArrivalTime;

                    }
                    newCustomer.RandomService = ServiceRandom.Next(1, 100);           
                    newCustomer.ServiceTime = CheckRange(newCustomer.AssignedServer.TimeDistribution, newCustomer.RandomService);
                    newCustomer.EndTime = newCustomer.StartTime + newCustomer.ServiceTime;
                    Servers[ServerIndex].last_finish_time = newCustomer.EndTime;
                }
                else
                {
                    //Time in queue
                    ServerIndex = MinFinishTime();
                    newCustomer.AssignedServer = Servers[ServerIndex];
                    newCustomer.StartTime = newCustomer.AssignedServer.last_finish_time;
                    newCustomer.TimeInQueue = newCustomer.StartTime - newCustomer.ArrivalTime;
                    newCustomer.RandomService = ServiceRandom.Next(1, 100);
                    newCustomer.ServiceTime = CheckRange(newCustomer.AssignedServer.TimeDistribution,newCustomer.RandomService);
                    newCustomer.EndTime = newCustomer.StartTime + newCustomer.ServiceTime;
                    newCustomer.ServerIndex = ServerIndex + 1;
                    Servers[ServerIndex].last_finish_time = newCustomer.EndTime;
                }
                SimulationTable.Add(newCustomer);
                lastCustomer = newCustomer;
                existingCustomerNumber++;
            }
            PerformanceMeasures.calculate_system_Performance(this);
            PerformanceMeasures.calculate_server_Performance(this);
        }

    }
}