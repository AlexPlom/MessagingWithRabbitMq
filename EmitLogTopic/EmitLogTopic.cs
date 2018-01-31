using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using DummyLibrary;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
            var obj = new BahyrWithBacon() { Name = "Bahyr", Description = "Delicious" };
            var body = obj.Serialize();

            channel.BasicPublish(exchange: "topic_logs",
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, "Bahyr bratle");
            Console.ReadLine();
        }
    }
}