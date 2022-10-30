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
    public partial class Graph : Form
    {
        SimulationSystem System;
        public Graph(SimulationSystem system)
        {
            this.System = system;
            InitializeComponent();
        }

        private void Graph_Load(object sender, EventArgs e)
        {
            for(int i = 1; i <= System.Servers.Count; i++)
            {
                for (int j = 0; j < System.StoppingNumber; j++)
                {
                    if (System.SimulationTable[j].ServerIndex == i)
                    {
                        int Start = System.SimulationTable[j].StartTime;
                        int End = System.SimulationTable[j].EndTime;
                        for (int c = Start; c <= End; c++)
                        {
                            this.chart1.Series["Series1"].Points.AddXY(c, 1);
                        }
                    }
                }
            }
        }
    }
}

