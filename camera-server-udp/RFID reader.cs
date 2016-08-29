using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsAlienRFID2;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;

namespace rfid_camera_server
{
    class RFID_reader
    {
        private clsReader mReader;
        //private ReaderInfo mReaderInfo;
        //private ComInterface meReaderInterface = ComInterface.enumTCPIP;
        int rfatt = 0; //Legal limits are between 0 and 150
        int RFLevel = 250; //Legal limits are between 166 and 316
        string response;
        worker wor;
        public string ipadd = "147.97.139.34";
        internal int portNumber = 23;
        List<string> tags;
        int numFoundtags = 0;
        List<int> ants;
        Form1 f;
       
        System.Windows.Forms.Timer ti;
        public RFID_reader(worker work, Form1 f1 )
        {
            f = f1;
            wor = work;
            mReader = new clsReader(true);
            mReader.KeepNetworkConnectionAlive = true;
            tags = new List<string>();
            ants = new List<int>();
            
            ti = new System.Windows.Forms.Timer();
            ti.Interval = 1000;
            ti.Tick += new System.EventHandler(this.ti_Tick);
            connect();




        }
        internal void connect()
        {
            String result;

            mReader.InitOnNetwork(ipadd, Convert.ToInt32(portNumber));
            
            result = mReader.Connect();

            if (!mReader.IsConnected)
            {


                MessageBox.Show("reader not connecting, is reader on?");
                return;
            }
            else
            {

                if (!mReader.Login("alien", "password"))		//returns result synchronously
                {

                    mReader.Disconnect();
                    return;
                }
                else
                {
                    mReader.FactorySettings();
                    response = mReader.SendReceive("set AntennaSequence = 0, 1, 2, 3", false);
                    mReader.SendReceive("set RFAttenuation = " + rfatt.ToString(), false);
                    mReader.SendReceive("RFLevel = " + RFLevel, false);
                    ti.Start();

                }

            }

        }
        public void polling()
        {

           
                
                        response = checkReader();

                        processRespose();
                       


             
           
        }
        private String checkReader()
        {
          
            try
            {

                return mReader.SendReceive("t", false);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
     


        }
        internal void processRespose()
        {
            tags.Clear();
            ants.Clear();
            //Thank you http:///txt2re.com/ !!!!!!!
            Match m;
            string txt = response;
            
          
            int i = 0, j = 0;
            int location = 0;
//"Tag:0000 0000 0000 0000 5024 6568, Disc:2013/11/14 17:09:07.970, Last:2013/11/14 17:09:07.970, Count:56, Ant:1, Proto:2\r\nTag:A5A5 1001 0100 0000 0000 0027, Disc:2013/11/14 17:10:46.662, Last:2013/11/14 17:10:46.662, Count:61, Ant:0, Proto:2\r\nTag:0000 0000 0000 0000 1028 7392, Disc:2013/11/14 17:10:58.281, Last:2013/11/14 17:10:58.281, Count:1, Ant:0, Proto:2\r\nTag:A5A5 1001 0100 0000 0000 0024, Disc:2013/11/14 17:11:58.272, Last:2013/11/14 17:11:58.272, Count:6, Ant:0, Proto:2\r\nTag:0000 0000 0000 0000 5024 4996, Disc:2013/11/14 17:12:35.358, Last:2013/11/14 17:12:35.358, Count:12, Ant:0, Proto:2\r\nTag:0000 0000 0000 0000 5027 2525, Disc:2013/11/14 17:16:56.057, Last:2013/11/14 17:16:56.057, Count:1, Ant:0, Proto:2\r\nTag:2DF0 0000 0000 0003 09E6 A2FC, Disc:2013/11/14 17:17:17.219, Last:2013/11/14 17:17:17.219, Count:3, Ant:0, Proto:2\r\nTag:0000 0000 0000 0000 5020 5051, Disc:2013/11/14 17:17:17.411, Last:2013/11/14 17:17:17.411, Count:2, Ant:0, Proto:2\r\nTag:A5A5 1001 0100 0000 0000 0023, Disc:2013/11/14 17:17:37.727, Last:2013/11/14 17:17:37.727, Count:2, Ant:0, Proto:2\r\nTag:0000 0000 0000 0000 5029 7279, Disc:2013/11/14 17:17:38.996, Last:2013/11/14 17:17:38.996, Count:24, Ant:1, Proto:2\r\nTag:0000 0000 0000 0000 3016 9716, Disc:2013/11/15 15:16:35.414, Last:2013/11/15 15:16:35.414, Count:1, Ant:1, Proto:2"
            do
            {

                string re1 = "(" + "tag:" + ")";
                Regex r = new Regex(re1, RegexOptions.IgnoreCase);// | RegexOptions.Singleline);
                m = r.Match(txt);

                if (m.Success)
                {
                    
                    location = m.Index;
                    ants.Add((int)Convert.ToInt32(txt.Substring(location + 108, 1)));
                    tags.Add(txt.Substring(location + 4, 29));
                    j++;
                    char[] arr = txt.ToArray<char>();
                    arr[location] = 'p';
                    txt = new string(arr);
                    
                }
                
                i++;
               
            }
            while (m.Success);


           try { f.Invoke(new MethodInvoker(delegate { wor.antsAndTagsFromReader(ants, tags); })); }
           catch (Exception ex) { };



            numFoundtags = j;

           
        }



        private void ti_Tick(object sender, EventArgs e)
        {
            Thread t = new Thread(polling);
            t.IsBackground = true;
            t.Start();

        }
    }
}
