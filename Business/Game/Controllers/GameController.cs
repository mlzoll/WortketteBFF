using Game.Business;
using Game.Controllers.Bodies;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Game.Controllers;

[Controller]
[Route("Game")]
public class GameController : ControllerBase
{
    private readonly IGameBusiness _gameBusiness;
    private readonly ILogger<GameController> _logger;

    public GameController(IGameBusiness gameBusiness, ILogger<GameController> logger)
    {
        _gameBusiness = gameBusiness;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewGame(
        [FromBody]CreateNewGameBody body)
    {
        (string gameId, string playerOneId, string playerTwoId) = await _gameBusiness.CreateNewGame(body.FirstWord, body.Topic);
        _logger.LogWarning("Warning from GameController");
        _logger.LogError("Error from GameController");
        _logger.LogInformation("Information from GameController");

        return Ok(new
                      {
                          PlayerOneId = playerOneId,
                          PlayerTwoId = playerTwoId,
                          GameId = gameId
                      });
    }
}