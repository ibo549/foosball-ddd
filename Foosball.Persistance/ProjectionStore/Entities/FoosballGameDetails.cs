using System;
using System.Collections.Generic;

namespace Foosball.Persistance.ProjectionStore.Entites
{
    public class FoosballGameDetails
    {
        public Guid Id { get; set;}
        public Dictionary<int, FoosballSet> Sets { get; set; }
        public FoosballGameDetails(Guid id)
        {
            Id = id;
            Sets = new Dictionary<int, FoosballSet>();
        }

        public void AddGoal(int setNum, string scorer, DateTimeOffset scoredAt)
        {
            if (Sets.ContainsKey(setNum))
                Sets[setNum].Goals.Add(new Goal(scorer, scoredAt));
            else 
            {
                var set = new FoosballSet(setNum);
                set.Goals.Add(new Goal(scorer, scoredAt));
                Sets.Add(setNum, set);
            }
        }
    }

    public class FoosballSet
    {
        public int SetNumber { get; }
        public IList<Goal> Goals { get; set; }

        public FoosballSet(int setNumber)
        {
            SetNumber = setNumber;
            Goals = new List<Goal>();
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