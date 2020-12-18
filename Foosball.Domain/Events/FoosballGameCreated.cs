using System;
using CQRSlite.Events;

namespace Foosball.Domain.Events
{
    public class FoosballGameCreated : IEvent 
	{
        public string TeamA {get; private set;}
        public string TeamB {get; private set;}
        public int SetsToWin {get; private set;}
        public FoosballGameCreated(Guid id, string teamA, string teamB, int setsToWin) 
        {
            Id = id;
            TeamA = teamA;
            TeamB = teamB;
            SetsToWin = setsToWin;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}