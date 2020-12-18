using System;
using CQRSlite.Events;

namespace Foosball.Domain.Events
{
    public class SetFinished : IEvent
    {
        public string Winner { get; private set; }
        public int SetNumber { get; private set; }
        public SetFinished(Guid id, string winner, int setNumber)
        {
            Id = id;
            Winner = winner;
            SetNumber = setNumber;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}