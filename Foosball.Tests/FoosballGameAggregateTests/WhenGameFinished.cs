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
    public class WhenGameFinished : Specification<FoosballGame, FoosballGameCommandHandlers, RecordGoal>
    {
        private Guid _gameId;
        private string _teamA = "teamA";
        private int _setsToWin = 2;
        protected override FoosballGameCommandHandlers BuildHandler()
        {
            return new FoosballGameCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _gameId = Guid.NewGuid();
            var events = new List<IEvent>
            {
                new FoosballGameCreated(_gameId, _teamA, "teamB", _setsToWin) {Version = 1}
            };

            //Score 10 goals for teamA in set 1
            for (int i = 2; i <= 11; i++)
            {
                events.Add(new GoalScored(_gameId, _teamA, 1) {Version = i});
            }
            events.Add(new SetFinished(_gameId, _teamA, 1) {Version = 12});
            
            //Score 9 goals for teamA in set 2
            for (int i = 13; i <= 21; i++)
            {
                events.Add(new GoalScored(_gameId, _teamA, 2) {Version = i});
            }


            return events;            
        }

        protected override RecordGoal When()
        {
            return new RecordGoal(_gameId, _teamA);
        }

        [Then]
        public void Should_create_three_events()
        {
            Assert.Equal(3, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<GoalScored>(PublishedEvents[0]);
            Assert.IsType<SetFinished>(PublishedEvents[1]);
            Assert.IsType<GameFinished>(PublishedEvents[2]);
        }

        [Then]
        public void Should_save_winning_team()
        {
            Assert.Equal(_teamA, ((GameFinished)PublishedEvents[2]).Winner);
        }

    }
}