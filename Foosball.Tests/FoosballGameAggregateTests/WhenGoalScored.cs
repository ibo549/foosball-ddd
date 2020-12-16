using System;
using System.Collections.Generic;
using System.Linq;
using Foosball.Domain.Commands;
using Foosball.Domain.CommandHandlers;
using Foosball.Domain.Events;
using CQRSlite.Events;
using Foosball.Domain.Tests.TestFixtures;
using Xunit;

namespace Foosball.Domain.Tests
{
    public class WhenGoalScored : Specification<FoosballGame, FoosballGameCommandHandlers, RecordGoal>
    {
        private Guid _gameId;
        private string _teamA = "teamA";
        private string _teamB = "teamB";
        private int _setsToWin = 2;
        protected override FoosballGameCommandHandlers BuildHandler()
        {
            return new FoosballGameCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
             _gameId = Guid.NewGuid();
            return new List<IEvent>
            {
                new FoosballGameCreated(_gameId, _teamA, _teamB, _setsToWin) {Version = 1}
            };
        }

        protected override RecordGoal When()
        {
            return new RecordGoal(_gameId, _teamA);
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<GoalScored>(PublishedEvents.First());
        }

        [Then]
        public void Should_save_scoring_team()
        {
            Assert.Equal(_teamA, ((GoalScored)PublishedEvents.First()).Scorer);
        }

        [Then]
        public void Should_save_correct_set_number()
        {           
            Assert.Equal(1, ((GoalScored)PublishedEvents.First()).SetNumber);
        }
    }
}