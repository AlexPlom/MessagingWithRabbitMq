using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class ReceiveLogs
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare("logs", "fanout");

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "logs", "");

            ListenForLogs(channel, queueName);
        }
    }

    private static void ListenForLogs(IModel channel, string queueName)
    {
        Console.WriteLine(" [*] Waiting for logs.");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] {0}", message);
        };
        channel.BasicConsume(queueName, true, consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}