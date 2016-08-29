using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfid_camera_server
{
    class user
    {
        internal string name;
        internal string RFID;
        internal int port;
        Form1 f;

        public user(Form1 f1, string inName, string inRFID, int inPort)
        {
            name = inName;
            RFID = inRFID;
            port = inPort;
            f = f1;


        }
    }
}
