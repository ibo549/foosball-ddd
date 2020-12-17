using System;
using Foosball.Persistance.ProjectionStore.Entites;
using CQRSlite.Queries;

namespace Foosball.Projections.Queries
{
    public class GetFoosballGameDetails : IQuery<FoosballGameDetails>
    {
        public GetFoosballGameDetails(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}