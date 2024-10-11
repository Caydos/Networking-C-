using SimpleTCP;

namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            if (Network.EstablishConnection("127.0.0.1", 5500))
            {
                Console.WriteLine("Connection established with server");

                // Sends an event to the server to say hello
                Network.TriggerServerEvent("testEvent");
            }
            EventManager.RegisterEvent("testEvent", testEvent);


            Console.ReadLine();
        }

        static void testEvent(string[] args)
        {
            /*
             * Parse arguments
             */
            Console.WriteLine("Event recieved from server");


            Network.TriggerServerEvent("testEvent", 1, 2, 3);
        }
    }
}