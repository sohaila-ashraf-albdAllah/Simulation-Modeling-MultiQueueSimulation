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
        List<TimeDistribution> list = new List<TimeDistribution>();
        struct inputs
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = String.Empty;
            string fileExe = String.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filePath = dialog.FileName;
                //Console.WriteLine(filePath);
               

            }
          
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines("C:\\Users\\aisha\\source\\repos\\sohaila-ashraf-albdAllah\\Simulation-Modeling-MultiQueueSimulation\\Template_Students\\MultiQueueSimulation\\MultiQueueSimulation\\TestCases\\TestCase1.txt");
            textBox1.Text = lines[4];
            textBox3.Text = lines[1];
            if (lines[7] == "1")
                radioButton4.Checked = true;
            else
                radioButton5.Checked = true;

            if (lines[10] == "1")
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
            textBox2.Text = "Time    probapility \n ";
            for (int i = 13; lines[i].Length!=0; i++)
            {
                TimeDistribution t = new TimeDistribution();
                string[] arr = lines[i].Split(',');
                t.Time = int.Parse(arr[0]);
                t.Probability = decimal.Parse(arr[1]);
                textBox2.Text+= t.Time.ToString()+"    " + t.Probability.ToString()+"\t";
                list.Add(t);
            }

            //textBox2.Text = "Time    probapility ";
            
            //for (int i=0;i<list.Count;i++)
            //    textBox2.Text += list[i].Time+"    "+ list[i].Probability+"\n";
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines("C:\\Users\\aisha\\source\\repos\\sohaila-ashraf-albdAllah\\Simulation-Modeling-MultiQueueSimulation\\Template_Students\\MultiQueueSimulation\\MultiQueueSimulation\\TestCases\\TestCase2.txt");
            textBox1.Text = lines[4];
            textBox3.Text = lines[1];
            if (lines[7] == "1")
                radioButton4.Checked = true;
            else
                radioButton5.Checked = true;

            if (lines[10] == "1")
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
            textBox2.Text = "Time    probapility \n ";
            for (int i = 13; lines[i].Length != 0; i++)
            {
                TimeDistribution t = new TimeDistribution();
                string[] arr = lines[i].Split(',');
                t.Time = int.Parse(arr[0]);
                t.Probability = decimal.Parse(arr[1]);
                textBox2.Text += t.Time.ToString() + "    " + t.Probability.ToString() + "\t";
                list.Add(t);
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines("C:\\Users\\aisha\\source\\repos\\sohaila-ashraf-albdAllah\\Simulation-Modeling-MultiQueueSimulation\\Template_Students\\MultiQueueSimulation\\MultiQueueSimulation\\TestCases\\TestCase3.txt");
            textBox1.Text = lines[4];
            textBox3.Text = lines[1];
            if (lines[7] == "1")
                radioButton4.Checked = true;
            else
                radioButton5.Checked = true;

            if (lines[10] == "1")
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
            textBox2.Text = "Time    probapility \n ";
            for (int i = 13; lines[i].Length != 0; i++)
            {
                TimeDistribution t = new TimeDistribution();
                string[] arr = lines[i].Split(',');
                t.Time = int.Parse(arr[0]);
                t.Probability = decimal.Parse(arr[1]);
                textBox2.Text += t.Time.ToString() + "    " + t.Probability.ToString() + "\t";
                list.Add(t);
            }
        }
    }
}
