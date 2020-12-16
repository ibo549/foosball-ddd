using System;
using CQRSlite.Commands;

namespace Foosball.Domain.Commands
{
    public class CreateGame : ICommand 
	{
        public CreateGame(Guid gameId, int setsToWin, string teamA, string teamB)
        {
            GameId = gameId;
            if(setsToWin <= 0) 
                throw new ArgumentException($"{nameof(setsToWin)} must be bigger than 0");
            
            if(string.IsNullOrEmpty(teamA) || string.IsNullOrEmpty(teamB))
                throw new ArgumentException("Team name can not be empty");

            SetsToWin = setsToWin;
            TeamA = teamA;
            TeamB = teamB;
        }
        public Guid GameId { get; private set; }
        public readonly int SetsToWin;
        public readonly string TeamA;	 
        public readonly string TeamB;	 	    
      
	}
}