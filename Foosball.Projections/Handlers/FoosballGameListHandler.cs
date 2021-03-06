using System.Collections.Generic;
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
	public class FoosballGameListHandler : ICancellableEventHandler<FoosballGameCreated>,
	    ICancellableEventHandler<SetFinished>,
        ICancellableQueryHandler<GetFoosballGames, IReadOnlyCollection<FoosballGameListItem>>
    {
        private readonly IProjectionStore _store;

        public FoosballGameListHandler(IProjectionStore store)
        {
            _store = store;
        }

        public Task Handle(FoosballGameCreated message, CancellationToken token)
        {
            _store.AddFoosballGameListItem(new FoosballGameListItem(message.Id, 
            message.TimeStamp, 
            message.SetsToWin, 
            message.TeamA, message.TeamB));
            return Task.CompletedTask;
        }

        public Task Handle(SetFinished message, CancellationToken token)
        {
            _store.AddSetWin(message.Id, message.Winner);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<FoosballGameListItem>> Handle(GetFoosballGames message, CancellationToken token = default)
        {
            return Task.FromResult(_store.GetFoosballGames());
        }
    }
}