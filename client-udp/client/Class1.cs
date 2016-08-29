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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Drawing.Imaging;
using System.IO;

using System.Net;
using System.Net.Sockets;


namespace WindowsFormsApplication3
{
    class reciever
    {
        Form1 f;
        Thread t; 
        Bitmap bitmap;
        List<Bitmap> bml;
        public reciever(Form1 f1)
        {
            bml = new System.Collections.Generic.List<Bitmap>();
            bml.Add(new Bitmap(640, 480));
            bml.Add(new Bitmap(640, 480));
            bml.Add(new Bitmap(640, 480));
          
            f = f1;
            t = new Thread(startListening);
            t.IsBackground = true;
            t.Start();
            f.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;


        }

        private void startListening()
        {
            ////////////////////////////////////////////

            byte[] data = new byte[1024];
            
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any , f.port);

            Socket newsock = new Socket(AddressFamily.InterNetwork,
                    SocketType.Dgram, ProtocolType.Udp);
            newsock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            newsock.Bind(ipep);
            //newsock.Listen(10);
            newsock.ReceiveBufferSize = 200000;
            Console.WriteLine("Waiting for a client...");
            
            
            //Socket client = newsock.Accept();
            newsock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
           // IPEndPoint newclient = (IPEndPoint)newsock.RemoteEndPoint;


            while (true)
            {   
                byte[] ba = new byte[40000];
               try
                {

                    newsock.Receive(ba);
                MemoryStream ms = new MemoryStream(ba);
               
                    //Image bmp = Image.FromStream(ms);
                    f.pictureBox1.Image = bml[2];
                    Image temp = Image.FromStream(ms);
                    bml.Add(new Bitmap( temp));
                    
                }
                catch (Exception e) {  }


                if (ba.Length == 0)
                    newsock.Listen(10);
                if (bml.Count > 3)
                {
                    bml.RemoveAt(0);
                }

            }

            // client.Close();
            //newsock.Close();
            /////////////////////////////////////////////

        }

        private static byte[] ReceiveVarData(Socket s)
        {
            int total = 0;
            int recv;
            byte[] datasize = new byte[4];
            try
            {
                recv = s.Receive(datasize, 0, 4, 0);
            }
            catch (Exception ex)
            {


            }
            int size = BitConverter.ToInt32(datasize, 0);
            int dataleft = size;
            byte[] data = new byte[size];


            while (total < size)
            {
                recv = s.Receive(data, total, dataleft, 0);
                if (recv == 0)
                {
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
        }
        internal void getScreen()
        {

            Rectangle bounds = Screen.GetBounds(Point.Empty);
            bitmap = new Bitmap(bounds.Width, bounds.Height);
            
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    f.temp = bitmap;
                }
                
            

        }

    }
}
