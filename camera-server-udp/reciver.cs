using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Drawing.Imaging;


namespace rfid_camera_server
{
    class reciever
    {
    
        Form1 f;
        Thread t; 
        Bitmap bitmap;
        internal int x = 0, y = 0;
        internal connection con;
        internal PictureBox pb;
        internal int port;
        Bitmap temp;
        int ant;

        public reciever(Form1 f1, int inPort)
        {
            f = f1;
            port = inPort;
            t = new Thread(startListening);
            t.IsBackground = true;
            t.Start();
            temp = new Bitmap(640, 480);
           // f.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            if (port < 6004)
            {
                ant = port - 6000;
            }
           
             
            
        }
       
        private void startListening()
        {
            
            byte[] data = new byte[200000];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);

            Socket newsock = new Socket(AddressFamily.InterNetwork,
                    SocketType.Dgram, ProtocolType.Udp);
            newsock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            newsock.Bind(ipep);
                newsock.ReceiveBufferSize = 200000;
                EndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                newsock.ReceiveFrom(data, ref ep);
                newsock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                int i = 0;
                for (; i < ep.ToString().Length && ep.ToString()[i] != ':'; i++) { }
                con = new connection(f, ep.ToString().Substring(0, i), port);
            IPAddress rip;
            IPAddress.TryParse(ep.ToString().Substring(0, i),out rip);
            EndPoint rep = new IPEndPoint(rip, port);
            
            while (true)
            {
                byte[] ba = new byte[40000];
                try
                {
                    newsock.Receive(ba);
                    MemoryStream ms = new MemoryStream(ba);
                    Image bmp = new Bitmap(Image.FromStream(ms));
                    try { f.Invoke(new MethodInvoker(delegate { f.pictureBox1.Image = bmp; })); }
                    catch (Exception ex) { };
                    newsock.SendTo(ba, rep);
                
                
                   
                   /* data d = new data();
                    d.port = port;
                    d.b = new Bitmap(bmp);
                    d.c = con.server;*/
                    //System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(go), (object)d);
                    
                }
                catch (Exception e) { }


                if (ba.Length == 0)
                {
                    try
                    {
                        newsock.Listen(10);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }

                //try { f.Invoke(new MethodInvoker(delegate { if (port > 6003) { pb.BringToFront(); } })); }
                //catch (Exception ex) { };
            }


            // client.Close();
            //newsock.Close();
            /////////////////////////////////////////////

        }

        
        internal void getScreen()
        {

            Rectangle bounds = Screen.GetBounds(Point.Empty);
            bitmap = new Bitmap(bounds.Width, bounds.Height);
            
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                   pb.Image = bitmap;
                }
                
            

        }
        public void go(Object state)
        {
            data dat = state as data;
            byte[] data = new byte[1024];

            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            try
            {

                MemoryStream ms = new MemoryStream();
                try
                {
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    System.Drawing.Imaging.Encoder myEncoder =
                        System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 30L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    dat.b.Save(ms, jgpEncoder, myEncoderParameters);

                }
                catch (Exception ex)
                { }
                byte[] bmpBytes = ms.ToArray();


                try
                {
                    //dat.c.Send(bmpBytes);
                }
                catch (Exception ex)
                {
                   

                }

            }
            catch (Exception x)
            {
            }
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

    }

public class  data
{
    public readonly Bitmap b;
    public data(Bitmap b1)
    {
        b = b1;
    }
}
}