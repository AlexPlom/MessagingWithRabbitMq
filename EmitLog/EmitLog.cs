using System;
using RabbitMQ.Client;
using System.Text;

class EmitLog
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare("logs", "fanout");

            PublishMessages(args, channel);
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    private static void PublishMessages(string[] args, IModel channel)
    {
        for (int i = 0; i < 10; i++)
        {
            var message = "Message " + i;
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("logs", "", null, body);

            Console.WriteLine("[x] Sent {0}", message);
        }
    }
}