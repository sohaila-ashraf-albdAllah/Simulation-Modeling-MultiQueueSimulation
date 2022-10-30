using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueModels;
using MultiQueueTesting;
namespace MultiQueueSimulation
{
    public partial class simulationTable : Form
    {
        public SimulationSystem System;
        public simulationTable(SimulationSystem System)
        {
            this.System = System;
            InitializeComponent();
        }

        private void simulationTable_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < System.StoppingNumber; i++)
            {
                dataGridView1.Rows.Add(System.SimulationTable[i].CustomerNumber, 
                    System.SimulationTable[i].RandomInterArrival, System.SimulationTable[i].InterArrival, 
                    System.SimulationTable[i].ArrivalTime, System.SimulationTable[i].RandomService, 
                    System.SimulationTable[i].RandomService, System.SimulationTable[i].ServiceTime, System.SimulationTable[i].ServerIndex,
                    System.SimulationTable[i].StartTime, System.SimulationTable[i].EndTime, System.SimulationTable[i].TimeInQueue);
            }
                
           
        }
    }
}
