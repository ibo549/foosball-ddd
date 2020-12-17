using System.Threading;
using System.Threading.Tasks;
using Foosball.Persistance.ProjectionStore.Entites;
using Foosball.Domain.Events;
using Foosball.Persistance.ProjectionStore;
using Foosball.Projections.Queries;
using CQRSlite.Events;
using CQRSlite.Queries;

namespace Foosball.Projections.Handlers
{
	public class FoosballGameDetailsHandler: ICancellableEventHandler<FoosballGameCreated>,
	    ICancellableEventHandler<GoalScored>,
	    ICancellableQueryHandler<GetFoosballGameDetails, FoosballGameDetails>
    {
        private readonly IProjectionStore _store;

        public FoosballGameDetailsHandler(IProjectionStore store)
        {
            _store = store;
        }

        public Task Handle(FoosballGameCreated message, CancellationToken token)
        {
            _store.AddFoosballGameDetails(new FoosballGameDetails(message.Id));
            return Task.CompletedTask;
        }

        public Task Handle(GoalScored message, CancellationToken token)
        {
            _store.AddGoal(message.Id, message.SetNumber, message.Scorer, message.TimeStamp);
            return Task.CompletedTask;
        }

        public Task<FoosballGameDetails> Handle(GetFoosballGameDetails message, CancellationToken token = default)
        {
            return Task.FromResult(_store.GetGameDetails(message.Id));
        }
    }
}