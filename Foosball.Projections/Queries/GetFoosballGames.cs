using System.Collections.Generic;
using Foosball.Persistance.ProjectionStore.Entites;
using CQRSlite.Queries;

namespace Foosball.Projections.Queries
{
    public class GetFoosballGames : IQuery<IReadOnlyCollection<FoosballGameListItem>>
    {
        
    }
}