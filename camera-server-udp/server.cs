using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;

using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Timers;

namespace rfid_camera_server
{
    class connection
    {
        System.Timers.Timer sertimer;
        Form1 f;
        public Socket server = null;
        IPEndPoint ipep;
        internal int index; 
        internal Bitmap temp = new Bitmap(320, 240);
        public Thread trd;
        int port;
        Bitmap localTemp;
       
        
        public connection(Form1 f1, string ipad, int inport)
        {
            f = f1;
            port = inport;
            index = f.work.rec.Count;
            sertimer = new System.Timers.Timer(95);
            sertimer.Enabled = true;
            sertimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            sertimer.Start();
            trd = new Thread(connectThreadFunction);
            
            stuff(ipad);
            trd.Start();
            trd.IsBackground = true;
            

        }
        
        public void stuff(string ipad)
        {
            ipep = new IPEndPoint(IPAddress.Parse(ipad), port);
            if (server == null)
            {
                server = new Socket(AddressFamily.InterNetwork,
                       SocketType.Dgram, ProtocolType.Udp);




            }



        }
        public void connectThreadFunction()
        {

            while (connect()) { }
        }
        public bool connect()
        {

            try
            {
                server.Connect(ipep);
            }
            catch (SocketException ex)
            {
                //MessageBox.Show("Unable to connect to server.");
                return true;
                //Console.WriteLine(e.ToString());

            }

            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            



            return false;

        }
        
        public void go()
        {
            ReaderWriterLockSlim rwls = new ReaderWriterLockSlim();
            byte[] data = new byte[1024];

            //try
            //{
               // rwls.EnterReadLock();
                localTemp = f.work.bitmapList[index][(((port-6000)+1)/2)];
                //localTemp = f.work.bitmapList[index][0];
               // rwls.ExitReadLock();
            //}
            //catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            try
            {

                MemoryStream ms = new MemoryStream();
                try
                {
                    //localTemp = temp;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    System.Drawing.Imaging.Encoder myEncoder =
            System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 30L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    localTemp.Save(ms, jgpEncoder, myEncoderParameters);
                   // localTemp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    
                }
                catch (Exception ex)
                { }
                byte[] bmpBytes = ms.ToArray();
                
       
                try
                {
                    server.Send(bmpBytes);
                   // sent = SendVarData(server, bmpBytes);
                }
                catch (Exception ex)
                {
                   /*  MessageBox.Show(ex.ToString());*/
                    if (ex.Message.ToLower().Contains("socketexception"))
                    {
                       // MessageBox.Show(ex.ToString());
                        server.Close();
                        //stuff();
                        trd.Abort();
                        trd = new Thread(connectThreadFunction);

                    }
                    
                }
             
            }
            catch (Exception x)
            {
                //MessageBox.Show(x.ToString());
                //while (true) ;
            }
           // f.work.bitmapList[index].RemoveAt(0);
        }

        
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;

        }
        private void sertimer_Tick(object sender, EventArgs e)
        {

            go();

        }
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Thread t = new Thread(go);
            t.IsBackground = true;
            t.Start();
        }

        

        public void send(Object state)
        {
            
        }
    }
}
