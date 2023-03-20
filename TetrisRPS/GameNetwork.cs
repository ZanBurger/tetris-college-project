using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TetrisRPS
{
    class GameNetwork
    {
        TcpListener? tcpListener;
        TcpClient? tcpClient;
        NetworkStream? networkStream;

        public GameNetwork() { }

        public void StartListener()
        {
            tcpListener = new TcpListener(IPAddress.Any, 8000);
            tcpListener.Start();
            tcpClient = tcpListener.AcceptTcpClient();
            networkStream = tcpClient.GetStream();
        }

        public void StopListener()
        {
            networkStream?.Close();
            tcpClient?.Close();
            tcpListener?.Stop();
        }

        public void StartClient(string ip)
        {
            tcpClient = new TcpClient(ip, 8000);
            networkStream = tcpClient.GetStream();
        }

        public void StopClient()
        {
            networkStream?.Close();
            tcpClient?.Close();  
        }

        public delegate void EventReceived(int e);
        public event EventReceived? OnEventReceived;
        byte[] data = new byte[4];

        public void BeginRead()
        {
            if (networkStream == null) return;
            networkStream?.BeginRead(data, 0, data.Length, OnReadComplete, null);
        }

        private void OnReadComplete (IAsyncResult ar)
        {
            try
            {
                int bytesRead = networkStream.EndRead(ar);
                if (bytesRead > 0)
                {
                    int ret = BitConverter.ToInt32(data);
                    OnEventReceived?.Invoke(ret);
                }
            }
            catch (Exception ex) { }
        }

        public void SendData(int key)
        {
            try
            {
                byte[] bytes = BitConverter.GetBytes(key);
                networkStream?.Write(bytes, 0, bytes.Length);
            }catch(Exception e) { }
        }  
    }
}
