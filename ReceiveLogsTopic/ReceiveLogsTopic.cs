using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

partial class ReceiveLogsTopic
{
    public static void Main(string[] args)
    {
        string[] arguments = new string[] { "*.info" };

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
            var queueName = channel.QueueDeclare().QueueName;

            foreach (var bindingKey in arguments)
            {
                channel.QueueBind(queue: queueName,
                                  exchange: "topic_logs",
                                  routingKey: bindingKey);
            }

            Console.WriteLine("Waiting for messages.. .");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgumenets) =>
            {
                var message = Deserializer.DeserializeToBacon(eventArgumenets.Body);
                var routingKey = eventArgumenets.RoutingKey;
                Console.WriteLine("Received '{0}':'{1} and description: {2}'", routingKey, message.Name, message.Description);
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine("Exit?");
            Console.ReadLine();
        }
    }
}