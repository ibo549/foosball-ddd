using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Domain;
using Foosball.Domain.Events;

namespace Foosball.Domain 
{
    public class FoosballGame : AggregateRoot 
    {
        private const int goalsToWinSet = 10;
        private int _setsToWin;
        private int _currentSetNumber;
        private string _teamA;
        private string _teamB;
        private IList<(int setNum, string scoringTeam)> _goalsScored = new List<(int, string)>();
        private FoosballGame() {}
        public FoosballGame(Guid id, string teamA, string teamB, int setsToWin)
        {
            Id = id;
            ApplyChange(new FoosballGameCreated(id, teamA, teamB, setsToWin));
        }

        public void RecordGoal(string scorer)
        {
            if(!IsTeamNameValid(scorer)) 
                throw new ArgumentException($"Invalid Goal Scorer: {scorer}");

            if(IsGameOver())
                throw new InvalidOperationException("Can't record a new goal, game is already over.");

            ApplyChange(new GoalScored(Id, scorer, _currentSetNumber));
            
            if(HasSetWon(scorer, _currentSetNumber))
            {
                ApplyChange(new SetFinished(Id, scorer, _currentSetNumber));

                if(IsGameOver())
                    ApplyChange(new GameFinished(Id, scorer));
            }

        }

        private bool IsTeamNameValid(string teamName)
        {
            return _teamA == teamName || _teamB == teamName;
        }

        private bool IsGameOver()
        {
           if(TotalSetsWon(_teamA) == _setsToWin || TotalSetsWon(_teamB) == _setsToWin)
                return true;

            return false;
        }

        private int TotalSetsWon(string team)
        {
            int setsWonSoFar = 0;

            for (int i = 1; i <= _currentSetNumber; i++)
            {
                if(HasSetWon(team, i))
                    setsWonSoFar++;
            }
            
            return setsWonSoFar;
        }

        private bool HasSetWon(string team, int setNum)
        {
            return _goalsScored
            .Count(g => g.scoringTeam == team && g.setNum == setNum) == goalsToWinSet;
        }

        #region Events
        private void Apply(FoosballGameCreated e)
        {
            _setsToWin = e.SetsToWin;
            _teamA = e.TeamA;
            _teamB = e.TeamB;
            _currentSetNumber = 1;
        }

        private void Apply(GoalScored e)
        {
            _goalsScored.Add((e.SetNumber, e.Scorer));
        }

         private void Apply(SetFinished e)
        {
            _currentSetNumber = e.SetNumber + 1;
        }
        #endregion
    }
}