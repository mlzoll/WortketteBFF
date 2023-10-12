namespace Game.Business;

public interface IGameBusiness
{
    Task<(string gameId, string playerOneId, string playerTwoId)> CreateNewGame(string firstWord, string Topic);

    Task<(List<string> words, string currentPlayerId, string topic)> GetCurrentGameState(string gameId, string playerId);

    Task<(bool accepted, string currentPlayerId)> SendPendingWord(string gameId, string playerId, string word);

    Task<(bool result, string currentPlayerId)> RejectPendingWord(string gameId, string playerId);

    Task<(bool result, string currentPlayerId)> AcceptPendingWord(string gameId, string playerId);

    Task<bool> CloseGame(string gameId, string playerId);
}