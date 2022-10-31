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
        public int id;
        public Graph(SimulationSystem system)
        {
            this.System = system;
            InitializeComponent();
        }

        private void Graph_Load(object sender, EventArgs e)
        {
            
            for(int i = 0; i < System.Servers.Count; i++)
            {
                comboBox1.Items.Add(System.Servers[i].ID);
               
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                //int servernum = System.Servers[x].ID;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            id= comboBox1.SelectedIndex;

            for (int j = 0; j < System.StoppingNumber; j++)
            {
                if (System.SimulationTable[j].ServerIndex == id + 1)
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

