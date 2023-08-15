using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;


namespace Server1
{
    public class main
    {
        public static void Main(string[] args)
        {
            try
            {
                Socket socket;
                int port = 8000;
                Server1.Server myserver = new Server1.Server(port);
                myserver.Create();
                Console.WriteLine("server started");

                
            } catch (Exception e)
            {
                Console.WriteLine("cannot create a web server", e.ToString());
            }
        }
    }
}
