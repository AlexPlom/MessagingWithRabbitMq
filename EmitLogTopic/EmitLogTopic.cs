using System;
using RabbitMQ.Client;
using DummyLibrary;

class EmitLogTopic
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "topic_logs",
                                    type: "topic");

            var routingKey = "info.info";
            PublishMessage(channel, routingKey);
        }
    }

    private static void PublishMessage(IModel channel, string routingKey)
    {
        var messageBody = new BahyrWithBacon() { Name = "Bahyr", Description = "Delicious" }.Serialize();

        channel.BasicPublish("topic_logs", routingKey, null, messageBody);
        Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, "Bahyr");
        Console.ReadLine();
    }
}