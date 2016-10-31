using System.Net;
using System.Net.Sockets;

namespace CallSystem.Class
{
   public class IP_PortSetting
    {
        private TcpClient objSck;
        private TcpListener server;

        public TcpListener IP_PortSet(string ipaddress, int port)
        {
            Dns.GetHostEntry(Dns.GetHostName());
            this.server = new TcpListener(IPAddress.Parse(ipaddress), port);
            this.server.Start();
            return this.server;
        }

        public NetworkStream SendIP_PortSet(string IPAdd, int port)
        {
            this.objSck = new TcpClient();
            this.objSck.ReceiveTimeout = 3000;
            this.objSck.Connect(IPAdd, port);
            return this.objSck.GetStream();
        }

        public NetworkStream DASSendIP_PortSet(string IPAdd, int port)
        {
            this.objSck = new TcpClient();
            this.objSck.SendTimeout = 500;
            this.objSck.Connect(IPAdd, port);
            return this.objSck.GetStream();
        }

        public void sendip_portset()
        {
            if (this.objSck == null)
                return;
            this.objSck.Close();
        }

        public void ServerStop()
        {
            if (this.server == null)
                return;
            this.server.Stop();
        }
    }
}
