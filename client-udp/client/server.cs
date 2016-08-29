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
//using AForge.Video;
//using AForge.Video.DirectShow;

namespace WindowsFormsApplication3
{
    class connection
    {
        Form1 f;
        Socket server = null;
        IPEndPoint ipep;
        public Thread trd;
        public connection(Form1 f1, string ipad, int port)
        {
            f = f1;
            trd = new Thread(connectThreadFunction);
            stuff(ipad, port);
            trd.Start();
            trd.IsBackground = true;
            

        }
       
        public void stuff(string ipad, int port)
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


            try { f.Invoke(new MethodInvoker(delegate { f.timer1.Start(); })); }
            catch (Exception ex) { ex.ToString(); };




            return false;

        }
        public void go()
        {

            // int screenWidth = Screen.GetBounds(new Point(0, 0)).Width;
            //int screenHeight = Screen.GetBounds(new Point(0, 0)).Height;
            // Bitmap bmpScreenShot = new Bitmap(screenWidth, screenHeight);
            //  Graphics gfx = Graphics.FromImage((Image)bmpScreenShot);
            // gfx.CopyFromScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));
            //bmpScreenShot.//.Save("c:\\a\\mytest.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            
            int sent;


            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            try
            {





                MemoryStream ms = new MemoryStream();
                // Save to memory using the Jpeg format
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                System.Drawing.Imaging.Encoder myEncoder =
        System.Drawing.Imaging.Encoder.Quality;
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 20L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                f.c.temp.Save(ms, jgpEncoder, myEncoderParameters);

                //f.textBox1.AppendText(ms.Length.ToString() + '\n');
                //bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                // read to end
                byte[] bmpBytes = ms.ToArray();

                //bmp.Dispose();
                //ms.Close();
                try
                {
                    server.Send(bmpBytes);
                }
                catch (Exception ex)
                {
                    // MessageBox.Show(ex.ToString());
                    if (ex.Message.ToLower().Contains("socketexception"))
                    {
                        MessageBox.Show(ex.ToString());
                        server.Close();
                        //stuff();
                        trd.Abort();
                        trd = new Thread(connectThreadFunction);

                    }

                }
                //Console.WriteLine("Disconnecting from server...");
                //server.Shutdown(SocketShutdown.Both);
                // server.Close();
                Console.ReadLine();
            }
            catch (Exception x)
            {
                //MessageBox.Show(x.ToString());
                //while (true) ;
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
}
