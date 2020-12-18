using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Queries;
using Foosball.Domain.Commands;
using Foosball.Persistance.ProjectionStore.Entites;
using Foosball.Projections.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foosball.WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FoosballGameController : ControllerBase
    {
        private const int _bo3SystemSetsToWin = 2;
        private readonly ICommandSender _commandSender;
        private readonly IQueryProcessor _queryProcessor;

        public FoosballGameController(ICommandSender commandSender, IQueryProcessor queryProcessor)
        {
            _commandSender = commandSender;
            _queryProcessor = queryProcessor;
        }

        [HttpPost]
        [Route("createGame")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateGame(string teamA, string teamB)
        {
            if (string.IsNullOrEmpty(teamA) || string.IsNullOrEmpty(teamB))
                return BadRequest();

            var gameId = Guid.NewGuid();
            await _commandSender.Send(new CreateGame(gameId, _bo3SystemSetsToWin, teamA, teamB));

            return Ok(gameId);
        }

        [HttpPut]
        [Route("addGoal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddGoal(Guid gameId, string scoringTeam)
        {
            if (string.IsNullOrEmpty(scoringTeam))
                return BadRequest($"{nameof(scoringTeam)} can't be null");

            try
            {
                await _commandSender.Send(new RecordGoal(gameId, scoringTeam));
            }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }

            return Ok();
        }


        [HttpGet]
        [Route("getGamesList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IReadOnlyCollection<FoosballGameListItem>> GetGamesList()
        {
            return await _queryProcessor.Query(new GetFoosballGames());
        }

        [HttpGet]
        [Route("getGameDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<FoosballGameDetails> GetGameDetails(Guid id)
        {
            return await _queryProcessor.Query(new GetFoosballGameDetails(id));
        }
    }
}
