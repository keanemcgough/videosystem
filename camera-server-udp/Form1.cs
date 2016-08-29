using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rfid_camera_server
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        internal worker work;
        private void Form1_Load(object sender, EventArgs e)
        {
            work = new worker(this);
            work.makeConnections();
            work.s = new settings(work, this);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            System.GC.Collect();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
        }

    }
}
