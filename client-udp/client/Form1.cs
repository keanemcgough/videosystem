using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AForge.Video.DirectShow;
using System.Windows.Forms;


using System.Drawing.Imaging;
using System.IO;

using System.Net;
using System.Net.Sockets;


namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {

        reciever r;
        internal Bitmap temp;
        internal connection con;
        internal IPAddress ip;
        private FilterInfoCollection videoDevices;
        internal camera c;
        internal int port;
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public Form1()
        {
            InitializeComponent();
            

            textBox1.Text = "147.97.138.89";
            textBox2.Text = "600";

            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            int i = 0;
            foreach (FilterInfo device in videoDevices)
            {
                if (!device.Name.ToLower().Contains("virtual"))
                {

                    c = new camera(videoDevices[i].MonikerString, this);
                }
                i++;
            }
            c.startCam();

        }
        internal void start()
        {
            r = new reciever(this);
            con = new connection(this, textBox1.Text, Convert.ToInt32( textBox2.Text));
            timer1.Start();

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            try
            {
                Thread goThread;
                goThread = new Thread(con.go);
                goThread.Start();
                goThread.IsBackground = true;
            }
            catch (Exception ex)
            { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                port = Convert.ToInt32( textBox2.Text);
                ip = IPAddress.Parse( textBox1.Text);
                start();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            c.CloseVideoSource();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
