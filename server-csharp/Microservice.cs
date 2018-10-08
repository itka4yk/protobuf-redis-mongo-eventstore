using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Eventstore;
using StackExchange.Redis;

namespace server_csharp
{
  public class Microservice : IHostedService
  {
    private readonly MongoClient Mongo;
    private readonly ConnectionMultiplexer Redis;

    public Microservice()
    {
      Mongo = new MongoClient();
      Redis = ConnectionMultiplexer.Connect("localhost");
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
      var db = this.Mongo.GetDatabase("events");

      var sub = Redis.GetSubscriber();
      sub.Subscribe("messages", (channel, message) =>
      {
        var @event = MessageEvent.Parser.ParseFrom(message);
        db.GetCollection<MessageEvent>("store").InsertOne(@event);
      });
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
      await Task.Run(() => Redis.Dispose());
    }
  }
}
