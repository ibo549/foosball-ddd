using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using LiteDB;

namespace Foosball.Persistance.EventStore
{
    public class LiteDbEventStore : IEventStore
    {
        private const string _dbName = "FoosballEventStore";
        private const string _eventsCollection = "events";
        private readonly IEventPublisher _publisher;

        public LiteDbEventStore(IEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
        {
            using (var db = new LiteDatabase(_dbName))
            {
                var collection = db.GetCollection<AggregateEvents>(_eventsCollection);
                foreach (var @event in events)
                {
                    var aggregateEvents = collection.FindById(@event.Id);
                    if (aggregateEvents == null)
                    {
                        aggregateEvents = new AggregateEvents(@event.Id);
                        aggregateEvents.Events.Add(@event);
                        collection.EnsureIndex(x => x.Id);
                        collection.Insert(aggregateEvents);
                    }
                    else
                    {
                        aggregateEvents.Events.Add(@event);
                        collection.Update(aggregateEvents);
                    }
                    await _publisher.Publish(@event, cancellationToken);
                }


            }

        }

        public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default)
        {
            using (var db = new LiteDatabase(_dbName))
            {
                var collection = db.GetCollection<AggregateEvents>(_eventsCollection);
                var events = collection.FindById(aggregateId)?.Events;
                return Task.FromResult(events?.Where(x => x.Version > fromVersion) ?? new List<IEvent>());
            }
              
        }
    }

    public class AggregateEvents
    {
        public Guid Id { get; set; }
        public List<IEvent> Events { get; set; }

        public AggregateEvents(Guid id)
        {
            Id = id;
            Events = new List<IEvent>();
        }
    }
}
