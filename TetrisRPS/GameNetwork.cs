using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TetrisRPS
{
    static class GameNetwork
    {
        static TcpListener tcpListener;
        static TcpClient tcpClient;
        static NetworkStream networkStream;

        public static async void StartListener()
        {
            tcpListener = new TcpListener(IPAddress.Any, 8000);
            tcpListener.Start();
            tcpClient = await tcpListener.AcceptTcpClientAsync();
            networkStream = tcpClient.GetStream();
        }

        public static void StopListener()
        {
            networkStream.Close();
            tcpClient.Close();
            tcpListener.Stop();
        }

        public static void StartClient(string ip)
        {
            tcpClient = new TcpClient(ip, 8000);
            networkStream = tcpClient.GetStream();
        }

        public static void StopClient(string ip)
        {
            networkStream.Close();
            tcpClient.Close();
        }

        public static async void GetData()
        {
            byte[] data = new byte[1024];
            int count = await networkStream.ReadAsync(data, 0, data.Length);


        }

        public static void SendData()
        {

        }
        
    }
}
