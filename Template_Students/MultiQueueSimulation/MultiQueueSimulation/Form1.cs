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
            string[] lines = File.ReadAllLines("TestCase1.txt");

            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
