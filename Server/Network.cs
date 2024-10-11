using SimpleTCP;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public static class Network
    {
        private static SimpleTcpServer tcpServer = new SimpleTcpServer();
        private static List<Socket> connectedClients = new List<Socket>();

        #region Booting
        public static void EstablishConnection(int port)
        {
            tcpServer.ClientConnected += OnClientConnected;
            tcpServer.ClientDisconnected += OnClientDisconnected;
            tcpServer.DataReceived += OnDataReceived;
            tcpServer.Start(port);
        }

        public static void CloseConnection()
        {
            tcpServer.ClientConnected -= OnClientConnected;
            tcpServer.ClientDisconnected -= OnClientDisconnected;
            tcpServer.DataReceived -= OnDataReceived;
            tcpServer.Stop();
        }
        #endregion

        #region Clients handlers
        private static void OnClientConnected(object sender, TcpClient e)
        {
            Console.WriteLine($"Client ({e.Client.RemoteEndPoint}) connected");
            connectedClients.Add(e.Client);
        }

        private static void OnClientDisconnected(object sender, TcpClient e)
        {
            Console.WriteLine($"Client ({e.Client.RemoteEndPoint}) disconnected");
            connectedClients.Remove(e.Client);
        }
        #endregion

        private static void OnDataReceived(object sender, Message e)
        {
            var ep = e.TcpClient.Client.RemoteEndPoint;
            var msg = Encoding.UTF8.GetString(e.Data);
            Console.WriteLine($"Received message from {ep} : \"{msg}\".");
            EventManager.TriggerRaw(msg);
        }

        public static void TriggerClientEvent(int clientId, string eventName, params object[] parameters)
        {
            if (!connectedClients[clientId].Connected)
            {
                Console.WriteLine($"Client {clientId} isn't connected to server");
                return;
            }
            connectedClients[clientId].Send(Encoding.UTF8.GetBytes(EventManager.Serialize(eventName, parameters)));
        }
    }
}
