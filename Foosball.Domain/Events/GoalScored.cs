using System;
using CQRSlite.Events;

namespace Foosball.Domain.Events
{
    public class GoalScored : IEvent
    {
        public string Scorer { get; private set; }
        public int SetNumber { get; private set; }
        public GoalScored(Guid id, string scorer, int setNumber)
        {
            Id = id;
            Scorer = scorer;
            SetNumber = setNumber;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}