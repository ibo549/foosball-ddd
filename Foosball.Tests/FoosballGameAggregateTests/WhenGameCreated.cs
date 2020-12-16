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
    public class WhenBoardCreated : Specification<FoosballGame, FoosballGameCommandHandlers, CreateGame>
    {
        private int _setsToWin = 2;
        private string _teamA = "teamA";
        private string _teamB = "teamB";
        protected override FoosballGameCommandHandlers BuildHandler()
        {
            return new FoosballGameCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            return new List<IEvent>();
        }

        protected override CreateGame When()
        {
            return new CreateGame(Guid.NewGuid(), _setsToWin, _teamA, _teamB);
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<FoosballGameCreated>(PublishedEvents.First());
        }

        [Then]
        public void Should_save_sets_to_win()
        {
            Assert.Equal(_setsToWin, ((FoosballGameCreated)PublishedEvents.First()).SetsToWin);
        }

        [Then]
        public void Should_save_team_names()
        {
            Assert.Equal(_teamA, ((FoosballGameCreated)PublishedEvents.First()).TeamA);
            Assert.Equal(_teamB, ((FoosballGameCreated)PublishedEvents.First()).TeamB);
        }
    }
}