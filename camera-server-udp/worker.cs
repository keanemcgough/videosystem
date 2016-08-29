using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace rfid_camera_server
{
    class worker
    {

        Thread m;
        List<RFID_reader> RFIDread;
        List<string> tags;
        List<int> ants;
        Form1 f;
        internal System.Timers.Timer protimer;
        internal int procam;
        internal List<reciever> rec;
        int ports = 6000;
        internal List<List<Bitmap>> bitmapList;
        internal List<List<BufferedWaveProvider>> bwplist;
        internal List<user> userList;
        internal settings s;
        public worker(Form1 f1)
        {

            //aud = new audio(f, this);
            tags = new List<string>();
            ants = new List<int>();
            f = f1;
            rec = new List<reciever>();
            bitmapList = new List<List<Bitmap>>();

            protimer = new System.Timers.Timer(30);
            protimer.Enabled = true;
            protimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            
            
            
            
           userList = new List<user>();
           
            RFIDread = new List<RFID_reader>();
            
            RFIDread.Add(new RFID_reader(this, f));
            s = new settings(this, f);
            bwplist = new List<List<BufferedWaveProvider>>();
        }
           
        internal void makeConnections()
        {
            
             for (int i = 0; i < 8; i++)
            {
                bitmapList.Add(new List<Bitmap>());
                 bwplist.Add(new List<BufferedWaveProvider>());
                rec.Add(new reciever(f, ports + i));
                for (int j = 0; j < 5; j++)
                {
                    bitmapList[i].Add(new Bitmap(640, 480));

                }

            }
            bitmapList.Add(new List<Bitmap>());
            bwplist.Add(new List<BufferedWaveProvider>());
            for (int j = 0; j < 5; j++)
            {
                bitmapList[8].Add(new Bitmap(320, 240));

            }
            protimer.Start();
        }
        
        public void doEverything()
        {
           for(int i = 0; i < userList.Count; i++)
           {

               if(userList[i].port >= 6004)
               {
                   for (int j = 0; j < tags.Count; j++)
                   {

                       if (tags[j] == userList[i].RFID && f.work.rec[userList[i].port - 6000].con != null && f.work.rec[ants[j]].con != null)
                       {
                           f.work.rec[userList[i].port - 6000].con.index = ants[j];
                           f.work.rec[ants[j]].con.index = userList[i].port - 6000;
                       }
                   }


               }
               
           }

           for (int i = 0; i < f.work.rec.Count; i++)
               {
                   bool t = false;
                   for (int j = 0; j < tags.Count; j++)
                   {
                       for (int l = 0; l < userList.Count; l++)
                       {

                           if (tags[j] == userList[l].RFID)
                           {
                               t = true;

                           }


                       }
                   }
                   if (t == false)
                   {
                       if (f.work.rec[i].con != null)
                       {
                           f.work.rec[i].con.index = 8;
                       }

                   }





               }

    
            



        }

        internal void antsAndTagsFromReader(List<int> afr, List<string> tfr)
        {

            ants = afr;
            tags = tfr;
            doEverything();
        }

       
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
         
        }

    }
}
