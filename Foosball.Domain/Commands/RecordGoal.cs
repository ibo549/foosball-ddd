using System;
using CQRSlite.Commands;

namespace Foosball.Domain.Commands
{
    public class RecordGoal : ICommand 
	{
        public RecordGoal(Guid gameId, string team)
        {
            GameId = gameId;
            
            if(string.IsNullOrEmpty(team))
                throw new ArgumentException($"{nameof(team)} can not be empty");

            ScoringTeam = team;
        }
        public Guid GameId { get; private set; }
        public readonly string ScoringTeam;	 	 	    
      
	}
}