using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
    class Send
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "docker-local.com" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("hello", false, false, false, null);

                string message = "Default";
                var body = Encoding.UTF8.GetBytes(message);

                while (true)
                {
                    channel.BasicPublish("", "hello", null, body);
                    Console.WriteLine(" [x] Sent {0}", message);
                    Console.ReadLine();
                }
            }
        }
    }
}
