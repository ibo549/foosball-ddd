using System;
using System.Collections.Generic;
using Foosball.Projections.Dtos;
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