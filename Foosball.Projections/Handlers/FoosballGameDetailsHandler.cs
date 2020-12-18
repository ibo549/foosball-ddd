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
	public class FoosballGameDetailsHandler: IEventHandler<FoosballGameCreated>,
	    IEventHandler<GoalScored>,
	    IQueryHandler<GetFoosballGameDetails, FoosballGameDetails>
    {
        private readonly IProjectionStore _store;

        public FoosballGameDetailsHandler(IProjectionStore store)
        {
            _store = store;
        }

        public Task Handle(FoosballGameCreated message)
        {
            _store.AddFoosballGameDetails(new FoosballGameDetails(message.Id));
            return Task.CompletedTask;
        }

        public Task Handle(GoalScored message)
        {
            _store.AddGoal(message.Id, message.SetNumber, message.Scorer, message.TimeStamp);
            return Task.CompletedTask;
        }

        public Task<FoosballGameDetails> Handle(GetFoosballGameDetails message)
        {
            return Task.FromResult(_store.GetGameDetails(message.Id));
        }
    }
}