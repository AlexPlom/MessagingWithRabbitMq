using RabbitMQ.Client;
using System;
using System.Text;

namespace NewTask
{
    class NewTask
    {
        private const int Message_Count = 10;

        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "task_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                PublishTaskMessages(args, channel, properties, Message_Count);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void PublishTaskMessages(string[] args, IModel channel, IBasicProperties properties, int messageCount)
        {
            for (int i = 0; i < messageCount; i++)
            {
                var message = "Task " + i;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties: properties, body: body);
                Console.WriteLine(" [x] Sent {0}", message);

                Console.WriteLine("Press [enter] to send the next message.");
                Console.ReadLine();
            }
        }
    }
}
