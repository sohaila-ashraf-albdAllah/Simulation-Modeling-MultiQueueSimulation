using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            int CurrentCustomer = 1;
            SimulationCase OldCase = new SimulationCase();
            Random random = new Random();
            Random ServiceRandom = new Random();
            Random ServerRandom = new Random();
            OldCase.ArrivalTime = 0;
            OldCase.InterArrival = 0;
            OldCase.EndTime = 0;
            bool cond = true;
            while (true)
            {
                //Our Main code!
                if (StoppingCriteria == (MultiQueueModels.Enums.StoppingCriteria.NumberOfCustomers )&& CurrentCustomer >StoppingNumber)
                {
                    break;
                }
                else if(StoppingCriteria == (MultiQueueModels.Enums.StoppingCriteria.SimulationEndTime) && TotalSimulationTime > StoppingNumber)
                {
                    break;
                }
                SimulationCase NewCase = new SimulationCase();
                NewCase.CustomerNumber = CurrentCustomer;
                NewCase.RandomInterArrival = random.Next(1, 101);
                NewCase.InterArrival = checkRange( NewCase.RandomInterArrival);

                if (CurrentCustomer == 1)
                    NewCase.ArrivalTime = OldCase.ArrivalTime;
                else
                    NewCase.ArrivalTime = OldCase.ArrivalTime + NewCase.InterArrival;
                //Server
                int ServerIndex = 0;
                if (CheckIdle(NewCase.ArrivalTime))
                {
                    NewCase.TimeInQueue = 0;
                    if (SelectionMethod == Enums.SelectionMethod.HighestPriority)
                    {
                        for (int i = 0; i < Servers.Count; i++)
                        {
                            if (Servers[i].LastFinishTime <= NewCase.ArrivalTime)
                            {
                                NewCase.AssignedServer = Servers[i];
                                ServerIndex = i;
                                break;
                            }
                        }
                    }
                    else if (SelectionMethod == Enums.SelectionMethod.Random)
                    {
                        int RandomServer = ServerRandom.Next(0, AvailableList.Count);
                        NewCase.AssignedServer = AvailableList[RandomServer];
                        ServerIndex = AvailableList[RandomServer].ID - 1;
                    }
                    NewCase.ServerIndex = ServerIndex + 1;
                    if (NewCase.AssignedServer.LastFinishTime > NewCase.ArrivalTime)
                    {
                        NewCase.StartTime = NewCase.AssignedServer.LastFinishTime;
                    }
                    else
                    {
                        NewCase.StartTime = NewCase.ArrivalTime;

                    }
                    NewCase.RandomService = ServiceRandom.Next(1, 101);
                    if (NewCase.RandomService == 0)
                    {
                        NewCase.RandomService = 1;
                    }
                    NewCase.ServiceTime = GetWithinRange(NewCase.AssignedServer.TimeDistribution, NewCase.RandomService);
                    NewCase.EndTime = NewCase.StartTime + NewCase.ServiceTime;
                    Servers[ServerIndex].LastFinishTime = NewCase.EndTime;
                }
                else
                {
                    //Time in queue
                    ServerIndex = GetFirstFinishServer();
                    NewCase.AssignedServer = Servers[ServerIndex];
                    NewCase.StartTime = NewCase.AssignedServer.LastFinishTime;
                    NewCase.TimeInQueue = NewCase.StartTime - NewCase.ArrivalTime;
                    NewCase.RandomService = ServiceRandom.Next(1, 101);
                    NewCase.ServiceTime = GetWithinRange(NewCase.AssignedServer.TimeDistribution, NewCase.RandomService);
                    NewCase.EndTime = NewCase.StartTime + NewCase.ServiceTime;
                    NewCase.ServerIndex = ServerIndex + 1;
                    Servers[ServerIndex].LastFinishTime = NewCase.EndTime;
                }
                SimulationTable.Add(NewCase);
                OldCase = NewCase;
                CurrentCustomer++;
            }

            //Don't forget PerformanceMeasures
            PerformanceMeasures.CalcServerPerformance(this);
            PerformanceMeasures.CalcSysPerformance(this);
            //System.PerformanceMeasures
            MessageBox.Show("End of Simulation");
        }
    }
}
