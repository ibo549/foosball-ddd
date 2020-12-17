using System;
using System.Linq;
using System.Collections.Generic;
using Foosball.Persistance.ProjectionStore.Entites;

namespace Foosball.Persistance.ProjectionStore
{
    public class InMemoryProjectionStore : IProjectionStore
    {
        private static List<FoosballGameListItem> _listItems = new List<FoosballGameListItem>();
        private static List<FoosballGameDetails> _details = new List<FoosballGameDetails>();
        public void AddFoosballGameDetails(FoosballGameDetails game)
        {
            _details.Add(game);
        }

        public void AddFoosballGameListItem(FoosballGameListItem game)
        {
            _listItems.Add(game);
        }

        public void AddGoal(Guid gameId, int setNumber, string scorer, DateTimeOffset scoredAt)
        {
           var game = _details.FirstOrDefault(x=> x.GameId == gameId);
           game?.AddGoal(setNumber, scorer, scoredAt);
        }

        public void AddSetWin(Guid gameId, string winningTeam)
        {
            var game = _listItems.FirstOrDefault();
            game?.AddSetWin(winningTeam);
        }

        public IReadOnlyCollection<FoosballGameListItem> GetFoosballGames()
        {
            return _listItems;
        }

        public FoosballGameDetails GetGameDetails(Guid id)
        {
            return _details.FirstOrDefault(x => x.GameId == id);
        }
    }
}