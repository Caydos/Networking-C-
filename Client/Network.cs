using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class Network
    {
        private static SimpleTcpClient tcpClient = new SimpleTcpClient();
        private static string ipAddress;
        private static int port;

        #region Booting
        public static bool EstablishConnection(string _ipAdress, int _port)
        {
            ipAddress = _ipAdress;
            port = _port;
            tcpClient.DataReceived += OnDataReceived;
            tcpClient.Connect(ipAddress, port);
            return tcpClient.TcpClient.Connected;
        }

        public static void CloseConnection()
        {
            tcpClient.DataReceived -= OnDataReceived;
            tcpClient.Disconnect();
        }
        #endregion

        #region Events handlers
        private static void OnDataReceived(object sender, Message e)
        {
            var ep = e.TcpClient.Client.RemoteEndPoint;
            var msg = Encoding.UTF8.GetString(e.Data);
            Console.WriteLine($"Received message from {ep} : \"{msg}\".");
            EventManager.TriggerRaw(msg);
        }

        public static void TriggerServerEvent(string eventName, params object[] parameters)
        {
            if (!tcpClient.TcpClient.Connected)
            {
                Console.WriteLine("Not connection to any servers ");
                return;
            }
            string paramStr = string.Join(",", parameters);
            string fullMessage = $"{eventName}|{paramStr}";
            tcpClient.Write(fullMessage);
        }
        #endregion
    }
}
