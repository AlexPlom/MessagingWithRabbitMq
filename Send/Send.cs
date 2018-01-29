using System;
using RabbitMQ.Client;
using System.Text;
namespace Send
{
    class Send
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory() { HostName = "docker-local.com" };

            while (Console.ReadLine() == "y")
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare("Sample Message", false, false, false, null);

                        string message = "Sample Message";

                        var body = Encoding.UTF8.GetBytes(message);


                        for (int i = 0; i < 10; i++)
                        {
                            channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);

                        }

                        Console.WriteLine(" [x] sent {0}", message);
                    }
                }

                Console.WriteLine("Type \"y\" to continue the loop.");
            }

        }
    }
}
