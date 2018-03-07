using System;
using RabbitMQ.Client;
using DummyLibrary;
using System.Runtime.Serialization.Formatters.Binary;

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

            var routingKey = (args.Length > 0) ? args[0] : "info.info";
            BinaryFormatter bf = new BinaryFormatter();
            var bahyrWithBacon = new BahyrWithBacon() { Name = "Bahyr", Description = "Delicious" };
            var body = bahyrWithBacon.Serialize();

            channel.BasicPublish("topic_logs", routingKey, null, body);
            Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, "Bahyr bratle");
            Console.ReadLine();
        }
    }
}