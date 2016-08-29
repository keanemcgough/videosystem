using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication3
{
    class camera
    {
        internal string dev = null;
        internal VideoCaptureDevice vcd = null;
        int fsx = 640, fsy = 480;
        int fr = 30;
        Form1 f;
        internal int numAnt = 1;
        internal int[] ant;
        internal int numTags = 1;
        internal string[] tags;
        internal System.Windows.Forms.PictureBox pb;
        internal Bitmap temp = new Bitmap(640, 480);
        internal string nickname;
        internal bool isActive = true;
        
        
        public camera(string devin, Form1 f1)
        {
            
            ant = new int[numAnt];
            
            
            tags = new string[numTags];

            tags[0] = "A5A5 1001 0100 0000 0000 0203";
            f = f1;
            dev = devin;
            vcd = new VideoCaptureDevice( dev);
            vcd.NewFrame += new NewFrameEventHandler(video_NewFrame);
            CloseVideoSource();
            vcd.DesiredFrameSize = new Size(fsx, fsy);
            vcd.DesiredFrameRate = fr;
            
            pb = new System.Windows.Forms.PictureBox();
            this.pb.BackColor = Color.Black;
            this.pb.Location = new System.Drawing.Point(0, 0);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(284, 262);
            this.pb.TabIndex = 0;
            this.pb.TabStop = false;
            pb.BackColor = new Color(  );
            pb.Visible = true;
            f.Controls.Add(pb);
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            
            //f.textBox1.AppendText(devin);

            
            
        }
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            
            //f.pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            temp = (Bitmap)eventArgs.Frame.Clone();
             
            try
            {
                //(Bitmap)eventArgs.Frame.Clone();
                if (isActive)
                {
                    
                    //temp = (Bitmap)eventArgs.Frame.Clone();
                }
                /*temp = (Bitmap)eventArgs.Frame.Clone();
                if (temp == null)
                {
                    temp = new Bitmap(640, 480);
                }
                else
                {
                 //   f.Invoke(new MethodInvoker(delegate { f.textBox1.Visible = true; })); 
                 //  f.Invoke(new MethodInvoker(delegate { f.textBox1.Text = dev; })); 
                    
                    //temp = new Bitmap(640, 480);

                }*/
                //pb.Image = temp;
            }
            catch (Exception e)
            {
                //MessageBox.Show( e.ToString());

            }
             
        }
        

        internal void write(ref StreamWriter  r)
        {
            r.AutoFlush = true;
            r.WriteLine("~");
            r.WriteLine(dev);
            r.WriteLine(nickname);
            for (int i = 0; i < numAnt; i++)
            {
                r.WriteLine(ant[i].ToString());
            }
            r.WriteLine('#');
            for (int i = 0; i < numTags; i++)
            {
                r.WriteLine(tags[i]);
            }

        }

        internal void changeSize(int x, int y)
        {
            //vcd.Stop();
            
            //vcd.DesiredFrameSize = new Size(x, y);
            //vcd.Start();
            //img.Size.Height = y;
            //img.Height = y;
            
        }


        internal void startCam()
        {

            vcd.Start();
        }
        

        internal void CloseVideoSource()
        {
            if (!(vcd == null))
                if (vcd.IsRunning)
                {
                    vcd.SignalToStop();
                    
                    vcd = null;
                }
        }

        ~camera()
        {

            CloseVideoSource();

        }

    }
}
