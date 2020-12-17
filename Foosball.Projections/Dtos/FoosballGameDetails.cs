using System;
using System.Collections.Generic;

namespace Foosball.Projections.Dtos
{
    public class FoosballGameDetails
    {
        public Guid GameId { get; }
        private Dictionary<int, FoosballSet> _sets { get; } = new Dictionary<int, FoosballSet>();
        public IReadOnlyDictionary<int, FoosballSet> Sets => _sets;
        public FoosballGameDetails(Guid gameId)
        {
            GameId = gameId;
        }

        public void AddGoal(int setNum, string scorer, DateTimeOffset scoredAt)
        {
            if (_sets.ContainsKey(setNum))
                _sets[setNum].Goals.Add(new Goal(scorer, scoredAt));
            else 
            {
                var set = new FoosballSet(setNum);
                set.Goals.Add(new Goal(scorer, scoredAt));
                _sets.Add(setNum, set);
            }
               
        }
    }

    public class FoosballSet
    {
        public int SetNumber { get; }
        public IList<Goal> Goals { get; } = new List<Goal>();

        public FoosballSet(int setNumber)
        {
            SetNumber = setNumber;
        }
    }

    public class Goal
    {
        public string Scorer { get; }
        public DateTimeOffset ScoredAt { get; }
        public Goal(string scorer, DateTimeOffset scoredAt)
        {
            Scorer = scorer;
            ScoredAt = scoredAt;
        }
    }
}