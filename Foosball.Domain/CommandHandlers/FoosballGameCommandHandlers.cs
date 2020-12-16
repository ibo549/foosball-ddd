using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using Foosball.Domain.Commands;

namespace Foosball.Domain.CommandHandlers 
{
    public class FoosballGameCommandHandlers :  
    ICommandHandler<CreateGame>, 
    ICommandHandler<RecordGoal>
    {
        private readonly ISession _session;

        public FoosballGameCommandHandlers(ISession session)
        {
            _session = session;
        }

        public async Task Handle(CreateGame message)
        {
            var foosballGame = new FoosballGame(message.GameId, message.TeamA, message.TeamB, message.SetsToWin);
            await _session.Add(foosballGame);
            await _session.Commit();
        }

        public async Task Handle(RecordGoal message)
        {
            var foosballGame = await _session.Get<FoosballGame>(message.GameId);
            foosballGame.RecordGoal(message.ScoringTeam);
            await _session.Commit();
        }

    }
}