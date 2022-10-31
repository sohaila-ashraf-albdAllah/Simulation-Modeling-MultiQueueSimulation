using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MultiQueueModels;
using MultiQueueTesting;

namespace MultiQueueSimulation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public SimulationSystem System = new SimulationSystem();
        public SimulationSystem System1 = new SimulationSystem();
        public List<Server> Servers = new List<Server>();
        public List<TimeDistribution> list = new List<TimeDistribution>();
        public int numberOfTestCase = 0;
        public string TestingResults;
        public void read_testCase(int n)
        {
            string path = string.Empty;
            path = ("C:\\Material 4 term 1\\M&S\\labs\\Template_Students\\Simulation-Modeling-MultiQueueSimulation\\Template_Students\\MultiQueueSimulation\\MultiQueueSimulation\\TestCases\\TestCase" + n + ".txt");
            string[] lines = File.ReadAllLines(path);
            numberOfTestCase = n;
            textBox1.Text = lines[4];
            textBox3.Text = lines[1];
            System.NumberOfServers = int.Parse(lines[1]);
            System.StoppingNumber = int.Parse(lines[4]);

            if (lines[7] == "1")
            {
                radioButton4.Checked = true;
                System.StoppingCriteria = Enums.StoppingCriteria.NumberOfCustomers;

            }
            else if (lines[7] == "2")
            {
                radioButton5.Checked = true;
                System.StoppingCriteria = Enums.StoppingCriteria.SimulationEndTime;
            }

            if (lines[10] == "1")//selection method 
            {
                radioButton1.Checked = true;
                System.SelectionMethod = Enums.SelectionMethod.HighestPriority;
            }
            else if (lines[10] == "2")
            {
                radioButton2.Checked = true;
                radioButton3.Checked = false;
                System.SelectionMethod = Enums.SelectionMethod.Random;
            }
            else
            {
                radioButton3.Checked = true;
                System.SelectionMethod = Enums.SelectionMethod.LeastUtilization;
            }
         
            int temp = 0;
            textBox2.Text = "Time    probapility \n ";
            for (int i = 13; lines[i].Length != 0; i++)
            {
                TimeDistribution t = new TimeDistribution();
                string[] arr = lines[i].Split(',');
                t.Time = int.Parse(arr[0]);
                t.Probability = decimal.Parse(arr[1]);
                textBox2.Text += t.Time.ToString() + "    " + t.Probability.ToString() + "\t";
                list.Add(t);
                temp = i;
            }
            temp += 3;
            System.InterarrivalDistribution = list;

            for (int j = 0; j < System.NumberOfServers; j++)

            {
                List<TimeDistribution> tlist = new List<TimeDistribution>();
                Server S = new Server();

                for (int g = temp;g < lines.Length; g++)
                {
                    TimeDistribution t = new TimeDistribution();
                    string[] arr = lines[g].Split(',');
                    if (arr.Length == 1) { break; }
                    t.Time = int.Parse(arr[0]);
                    t.Probability = decimal.Parse(arr[1]);
                    textBox2.Text += t.Time.ToString() + "    " + t.Probability.ToString() + "\t";
                    tlist.Add(t);
                    temp = g;
                }
                S.TimeDistribution = tlist;
                S.ID = j + 1;
                Servers.Add(S);
                temp += 3;
            }
            System.Servers = Servers;
        }
        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            read_testCase(1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            read_testCase(2);
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            read_testCase(3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Graph graph = new Graph(System);
            graph.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            simulationTable table = new simulationTable(System);
            table.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton3.Checked = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            System.Build_Distribution_table(System.InterarrivalDistribution);
            System.BuildServerTable(System.Servers);
            System.Simulate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch(numberOfTestCase)
            {
                case 1:
                    {
                        TestingResults = TestingManager.Test(System, Constants.FileNames.TestCase1);
                        MessageBox.Show(TestingResults);
                        break;
                    }             
                case 2:
                    {
                        TestingResults = TestingManager.Test(System, Constants.FileNames.TestCase2);
                        MessageBox.Show(TestingResults);
                        break;
                    }                
                case 3:
                    {
                        TestingResults = TestingManager.Test(System, Constants.FileNames.TestCase3);
                        MessageBox.Show(TestingResults);
                        break;
                    }
            }
        }
    }
}
