namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            Network.EstablishConnection(5500);

            EventManager.RegisterEvent("testEvent", testEvent);

            Console.ReadLine();
        }

        static void testEvent(string[] args)
        {
            /*
             * Parse arguments
             */
            Console.WriteLine("Event called");
        }
    }
}