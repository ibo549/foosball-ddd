using System;

namespace Foosball.Persistance.ProjectionStore.Entites
{
    public class FoosballGameListItem
    {
        public Guid Id { get; set;}
        public DateTimeOffset StartDate { get; }
        public int SetsToWin { get; }
        public string TeamAName { get;}
        public string TeamBName { get; }
        public int TeamASetWins { get; private set;}
        public int TeamBSetWins { get; private set; }
        public FoosballGameListItem(Guid id, DateTimeOffset startDate, int setsToWin, 
        
        string teamAName, string teamBName)
        {
            Id = id;
            StartDate = startDate;
            SetsToWin = setsToWin;
            TeamAName = teamAName;
            TeamBName = teamBName;
        }
       
        public void AddSetWin(string teamName)
        {
            if(teamName == TeamAName)
                TeamASetWins++;
            else if (teamName == TeamBName)
                TeamBSetWins++;
            else
                throw new ArgumentException($"Invalid {nameof(teamName)}");
        }

    }
}