using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Connection
{
    public class ServerInfo
    {
        private static string ip, port;

        public static string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        public static string Port
        {
            get { return port; }
            set { port = value; }
        }
    }
}
