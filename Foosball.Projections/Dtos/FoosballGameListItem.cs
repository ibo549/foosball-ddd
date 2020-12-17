using System;

namespace Foosball.Projections.Dtos
{
    public class FoosballGameListItem
    {
        public FoosballGameListItem(Guid gameId, DateTimeOffset startDate, int setsToWin, 
        string teamAName, string teamBName)
        {
            GameId = gameId;
            StartDate = startDate;
            SetsToWin = setsToWin;
            TeamAName = teamAName;
            TeamBName = teamBName;
        }
        public Guid GameId { get;}
        public DateTimeOffset StartDate { get; }
        public int SetsToWin { get; }
        public string TeamAName { get;}
        public string TeamBName { get; }
        public int TeamASetWins { get; set; }
        public int TeamBSetWins { get; set; }

    }
}