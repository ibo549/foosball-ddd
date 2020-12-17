using System;
using System.Collections.Generic;
using Foosball.Persistance.ProjectionStore.Entites;

namespace Foosball.Persistance.ProjectionStore
{
    public interface IProjectionStore 
    {
        void AddFoosballGameListItem(FoosballGameListItem game);
        void AddSetWin(Guid gameId, string winningTeam);
        void AddFoosballGameDetails(FoosballGameDetails game);
        void AddGoal(Guid gameId, int setNumber, string scorer, DateTimeOffset scoredAt);
        IReadOnlyCollection<FoosballGameListItem> GetFoosballGames();
        FoosballGameDetails GetGameDetails(Guid id);
    }
}