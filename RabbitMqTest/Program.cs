// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Game.Dto;

using MongoDB.Bson.IO;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
                  {
                      Uri = new Uri("amqps://localhost:4700"),
                      UserName = "",
                      Password = ""
                  };

var connection = factory.CreateConnection();
var channel = connection.CreateModel();
var queue = channel.QueueDeclarePassive("testqueue1");
channel.QueueDeclare("testqueue1", false, false, false, null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
    {
        var result = JsonSerializer.Deserialize<GameData>(args.Body.ToArray());
        Console.WriteLine(result.PendingWord);
    };

var data = new GameData()
               {
                   CurrentPlayer = Player.PlayerTwo,
                   Id = "eine Id",
                   PendingWord = "was cooles"
               };
var bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(data);
channel.BasicPublish("", "testqueue1", null, bytes);
channel.BasicConsume("testqueue1", true, consumer);
Console.ReadLine();