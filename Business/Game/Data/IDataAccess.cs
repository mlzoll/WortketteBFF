using Game.Dto;

namespace Game.Data;

public interface IDataAccess
{
    Task<bool> CheckIfIdExists(string id);

    Task CreateNewGame(string gameId, string topic, string firstWord, string playerOneId, string playerTwoId);

    Task<GameData?> GetGame(string gameId);

    Task UpdateGame(GameData game);
}