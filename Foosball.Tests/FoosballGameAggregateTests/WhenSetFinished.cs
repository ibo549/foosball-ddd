using System;
using System.Collections.Generic;
using Foosball.Domain.Commands;
using Foosball.Domain.CommandHandlers;
using Foosball.Domain.Events;
using CQRSlite.Events;
using Foosball.Domain.Tests.TestFixtures;
using Xunit;

namespace Foosball.Domain.Tests
{
    public class WhenSetFinished : Specification<FoosballGame, FoosballGameCommandHandlers, RecordGoal>
    {
        private Guid _gameId;
        private string _teamA = "teamA";
        private int _setNumber = 1;
        protected override FoosballGameCommandHandlers BuildHandler()
        {
            return new FoosballGameCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _gameId = Guid.NewGuid();
            var events = new List<IEvent>
            {
                new FoosballGameCreated(_gameId, _teamA, "teamB", 2) {Version = 1}
            };

            //Score 9 goals for teamA
            for (int i = 2; i <= 10; i++)
            {
                events.Add(new GoalScored(_gameId, _teamA, _setNumber) {Version = i});
            }
            return events;            
        }

        protected override RecordGoal When()
        {
            return new RecordGoal(_gameId, _teamA);
        }

        [Then]
        public void Should_create_two_events()
        {
            Assert.Equal(2, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<GoalScored>(PublishedEvents[0]);
            Assert.IsType<SetFinished>(PublishedEvents[1]);
        }

        [Then]
        public void Should_save_winning_team()
        {
            Assert.Equal(_teamA, ((SetFinished)PublishedEvents[1]).Winner);
        }

        [Then]
        public void Should_save_correct_set_number()
        {           
            Assert.Equal(_setNumber, ((SetFinished)PublishedEvents[1]).SetNumber);
        }
    }
}