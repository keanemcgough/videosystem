using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace client
{
    class login
    {

        TcpClient tcpCon;
        IPAddress ipadd;
        int port;
        public login(IPAddress ip, int inPort)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(connect));
            ipadd = ip;
            port = inPort;
        }

        void connect(Object stateInfo)
        {
            
            tcpCon = new TcpClient(ipadd.ToString(), port);

            //tcpCon.Client.Send(
        }


    }
}
