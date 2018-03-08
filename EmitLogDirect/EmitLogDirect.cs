using System;
using RabbitMQ.Client;
using System.Text;

class EmitLogDirect
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");
            var severity = "info";

            for (int i = 0; i < 10; i++)
            {
                if (i < 5) PublishMessage(channel, severity);
                else PublishMessage(channel, "bahyr");
            }
        }

        WaitForExit();
    }

    private static void WaitForExit()
    {
        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    private static void PublishMessage(IModel channel, string severity)
    {
        var message = "Hello World!";

        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "direct_logs",
                             routingKey: severity,
                             basicProperties: null,
                             body: body);
        Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);
    }
}