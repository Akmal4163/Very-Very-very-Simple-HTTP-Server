using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Server1
{
    public class Server
    {
        Socket socket;
        int port;
        IPHostEntry host;
        IPAddress address;
        IPEndPoint localendpoint;

        String data = null;
        Byte[] bytes = null;

        string currentDate = null;
        String[] arrayofdata = null;

        string response = null;

        public Server(int port)
        {
            this.port = port;
            this.host = Dns.GetHostEntry("localhost");
            this.address = host.AddressList[0];
            this.localendpoint = new IPEndPoint(address, this.port);
        }
        public void Create()
        {
            Socket server_socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            server_socket.Bind(localendpoint);

            Console.WriteLine("waiting for a connection");

            server_socket.Listen(10);

            while (true)
            {
                Socket client_socket = server_socket.Accept();
                Task.Run(() => handlerequest(client_socket));
            }

        }

        public string encodetobase64(String data)
        {
            byte[] textbytes = Encoding.ASCII.GetBytes(data);
            return System.Convert.ToBase64String(textbytes);
        }

        public void handlerequest(Socket client_socket)
        {
            try
            {
                bytes = new byte[1024];
                int bytes_received = client_socket.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytes_received);
                Console.WriteLine(data);


                currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                arrayofdata = data.Split(" ");
                if (arrayofdata[0] == "GET")
                {
                    response = "<html><h1>Hello from C# server</h1><p>Method: GET</p><p>Current date and time: " + currentDate + "</p></html>";

                }
                else if (arrayofdata[0] == "POST")
                {
                    //response = "Hello from C# server, Method: POST, Current date and time: " + currentDate;
                    response = encodetobase64(arrayofdata[14]);
                    //foreach(string data in arrayofdata)
                    //{
                    //    Console.WriteLine(data);
                    //}
                    //Console.WriteLine(arrayofdata[14]);
                }

                string httpResponse = "HTTP/1.1 200 OK\r\n"
                        + "Content-Type: text/html\r\n"
                        + "Content-Length: " + response.Length + "\r\n"
                        + "\r\n"
                        + response;

                try
                {
                    byte[] messagetoclient = Encoding.ASCII.GetBytes(httpResponse);
                    client_socket.Send(messagetoclient);
                    Console.WriteLine("sending data success");
                    client_socket.Shutdown(SocketShutdown.Both);
                    client_socket.Close();
                }
                catch
                {
                    Console.Write(httpResponse);
                }
            } catch
            {
                Console.WriteLine("cannot handle request");
            }

        }
    }
}
