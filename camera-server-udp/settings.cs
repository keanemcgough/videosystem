using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Windows.Forms;

namespace rfid_camera_server
{
    
    class settings
    {
        Form1 f1;
        internal int numTags = 0;
        internal List<string> userTags;
        internal string IPaddr;
        internal int port;
        internal List<string> RFID;
        internal worker w;
        internal List<Int32> ant;
        internal int xres = 640, yres = 480, rr = 20;
        internal int power = 150, atten = 166;
        public settings(worker w1 , Form1 f)
        {
            xres = 640; yres = 480; rr = 20;
            userTags = new List<string>();
            f1 = f;
            w = w1;
            RFID = new List<string>();
            
            ant = new List<int>();
            readSettings();
        }
        internal void readSettings()
        {
            
            StreamReader settings = new StreamReader("settings.cfg");
            try { loadSettings(ref settings); }
            catch (Exception e) {  }
         

            
            
            
            settings.Close();

        }
        internal void writeSetttings()
        {

            StreamWriter settings = new StreamWriter("settings.cfg");
            try { saveSettings(ref settings); }
            catch (Exception e) { }
         
            settings.Close();
        }
        
       
       
        internal void loadSettings(ref StreamReader r)
        {
            
            IPaddr = r.ReadLine();
            port = Convert.ToInt32(r.ReadLine());
            xres = Convert.ToInt32(r.ReadLine());
            yres = Convert.ToInt32(r.ReadLine());
            rr = Convert.ToInt32(r.ReadLine());
            while (!r.EndOfStream)
            {

                string n = r.ReadLine();
                string rf = r.ReadLine();
                int p = Convert.ToInt32( r.ReadLine());

                // .Add(new user(n, rf, p));
                w.userList.Add(new user(f1, n, rf, p));

              
             

            }

          return;


        }
        internal void saveSettings(ref StreamWriter r)
        {


            r.WriteLine(IPaddr);
            r.WriteLine(port);
            r.WriteLine(xres);
            r.WriteLine(yres);
            r.WriteLine(rr);
            for (int i = 0; i < f1.work.userList.Count; i++)
            {
                user temp = f1.work.userList[i];
                r.WriteLine(temp.name);
                r.WriteLine(temp.RFID);
                r.WriteLine(temp.port);


            }
           
            



        }
    }
}
