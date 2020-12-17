using System;
using System.Collections.Generic;
using Foosball.Projections.Dtos;

namespace Foosball.Projections.Infrastructure
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