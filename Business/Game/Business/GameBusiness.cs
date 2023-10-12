using Game.Data;
using Game.Dto;

namespace Game.Business;

public class GameBusiness : IGameBusiness
{
    private readonly IDataAccess _dataAccess;

    public GameBusiness(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }
    public async Task<(string gameId, string playerOneId, string playerTwoId)> CreateNewGame(string firstWord, string topic)
    {
        string gameId = $"{Guid.NewGuid()}";
        string playerOneId = $"{Guid.NewGuid()}";
        string playerTwoId = $"{Guid.NewGuid()}";
        if (await _dataAccess.CheckIfIdExists(gameId))
            return await CreateNewGame(firstWord, topic);
        _ = _dataAccess.CreateNewGame(gameId, topic, firstWord, playerOneId, playerTwoId);
        return (gameId, playerOneId, playerTwoId);
    }

    public async Task<(List<string> words, string currentPlayerId, string topic)> GetCurrentGameState(string gameId, string playerId)
    {
        GameData? game = await _dataAccess.GetGame(gameId);
        if (game is null ||
            game.PlayerOneId != playerId &&
            game.PlayerTwoId != playerId)
            return (new List<string>(), "", "");
        return (game.Words, game.CurrentPlayerId(), game.Topic);
    }

    public async Task<(bool accepted, string currentPlayerId)> SendPendingWord(string gameId, string playerId, string word)
    {
        GameData? game = await _dataAccess.GetGame(gameId);
        if (game is null ||
            game.CurrentPlayerId() != playerId)
            return (false, "");
        game.PendingWord = word;
        game.SwitchCurrentPlayer();
        _ = _dataAccess.UpdateGame(game);
        return (true, game.CurrentPlayerId());
    }

    public async Task<(bool result, string currentPlayerId)> RejectPendingWord(string gameId, string playerId)
    {
        GameData? game = await _dataAccess.GetGame(gameId);
        if (game is null ||
            game.CurrentPlayerId() != playerId)
            return (false, "");
        game.PendingWord = null;
        game.SwitchCurrentPlayer();
        _ = _dataAccess.UpdateGame(game);
        return (true, game.CurrentPlayerId());
    }

    public async Task<(bool result, string currentPlayerId)> AcceptPendingWord(string gameId, string playerId)
    {
        GameData? game = await _dataAccess.GetGame(gameId);
        if (game is null ||
            game.CurrentPlayerId() != playerId)
            return (false, "");
        game.Words.Add(game.PendingWord!);
        game.PendingWord = null ;
       // No switch ich, bleibe der Spieler
       // aber aus Konsistenz Gründen, soll das das BFF entscheiden und nicht der Client
        _ = _dataAccess.UpdateGame(game);
        return (true, game.CurrentPlayerId());
    }

    public async Task<bool> CloseGame(string gameId, string playerId)
    {
        GameData? game = await _dataAccess.GetGame(gameId);
        return game is not null &&
               (game.PlayerOneId == playerId ||
                game.PlayerTwoId == playerId);
    }
}