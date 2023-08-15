using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server1
{
    public class Server
    {
        Socket socket;
        int port;
        IPHostEntry host;
        IPAddress address;
        IPEndPoint localendpoint;

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

            server_socket.Listen(10);

            Console.WriteLine("waiting for a connection");
            Socket client_socket = server_socket.Accept();

            String data = null;
            Byte[] bytes = null;

            while (true)
            {
                bytes = new byte[1024];
                int bytes_received = client_socket.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytes_received);
                Console.WriteLine(data);
                if(data.Contains("GET"))
                {
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string response = "<html><h1>Hello from C# server</h1><p>Method: GET</p><p>Current date and time: " + currentDate + "</p></html>";

                    string httpResponse = "HTTP/1.1 200 OK\r\n"
                        + "Content-Type: text/html\r\n"
                        + "Content-Length: " + response.Length + "\r\n"
                        + "\r\n"
                        + response;

                    //String senderdata =;
                    try
                    {
                        byte[] messagetoclient = Encoding.ASCII.GetBytes(httpResponse);
                        client_socket.Send(messagetoclient);
                        Console.WriteLine("sending data success");
                    }
                    catch
                    {
                        Console.Write(httpResponse);
                    }

                }

                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }
            Console.WriteLine("bytes received: ", bytes, "data :", data);

            client_socket.Shutdown(SocketShutdown.Both);
            client_socket.Close();
        }
    }
}
