using System;
using CQRSlite.Events;

namespace Foosball.Domain.Events
{
    public class GameFinished : IEvent 
	{
        public  string Winner {get; private set;}
        public GameFinished(Guid id, string winner) 
        {
            Id = id;
            Winner = winner;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}