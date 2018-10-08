using System;
using System.IO;
using StackExchange.Redis;
using Eventstore;
using Google.Protobuf;

// using Google.Protobuf;

namespace client
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello Client!");
      var redis = ConnectionMultiplexer.Connect("localhost");
      var sub = redis.GetSubscriber();
      var i = 0;
      while (true)
      {
        var a = Console.ReadKey();
        MessageEvent message = new MessageEvent
        {
          Id = i,
          Name = "sample event from c#",
          FiredDate = DateTime.Now.ToString()
        };
        // var mes = new ByteStr
        using (var stream = new MemoryStream())
        {
          message.WriteTo(stream);
          sub.Publish("messages", stream.ToArray());
        }

        Console.WriteLine("SENT message no: " + i);
        i++;
      }
    }
  }
}