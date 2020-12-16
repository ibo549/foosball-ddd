using System;
using CQRSlite.Events;

namespace Foosball.Domain.Events
{
    public class GoalScored : IEvent 
	{
        public readonly string Scorer;
        public readonly int SetNumber;
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