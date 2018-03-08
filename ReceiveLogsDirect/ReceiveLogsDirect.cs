using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class ReceiveLogsDirect
{
    public static void Main(string[] args)
    {
        args = new string[] { "info", "error", "debug" };

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            string queueName = ConfigureQueue(args, channel);

            ListenForDirectLogs(channel, queueName);
        }
    }

    private static void ListenForDirectLogs(IModel channel, string queueName)
    {
        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;
            Console.WriteLine(" [x] Received '{0}':'{1}'",
                              routingKey, message);
        };
        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");

        Console.ReadLine();
    }

    private static string ConfigureQueue(string[] args, IModel channel)
    {
        channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");
        var queueName = channel.QueueDeclare().QueueName;

        foreach (var severity in args)
        {
            channel.QueueBind(queue: queueName,
                              exchange: "direct_logs",
                              routingKey: severity);
        }

        return queueName;
    }
}