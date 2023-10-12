using Game.Dto;

using MongoDB.Driver;

namespace Game.Data;

public class DataAccess : IDataAccess
{
    private readonly IMongoCollection<GameData> _data;

    public DataAccess(GameConfiguration config)
    {
        MongoClient client = new MongoClient(config.ConnectionString);
        IMongoDatabase db = client.GetDatabase(config.MongoDbName);
        _data = db.GetCollection<GameData>(config.GameCollectionName);

    }
    public async Task<bool> CheckIfIdExists(string id) => await _data.FindAsync(x => x.Id == id).FirstOrDefaultAsync() is not null;

    public Task CreateNewGame(string gameId, string topic, string firstWord, string playerOneId, string playerTwoId)
    {
        return _data.InsertOneAsync(
            new()
            {
                Id = gameId,
                PlayerOneId = playerOneId,
                PlayerTwoId = playerTwoId,
                CurrentPlayer = Player.PlayerTwo,
                Topic = topic,
                Words = {firstWord}
            });
    }

    public async Task<GameData?> GetGame(string gameId)
    {
        IAsyncCursor<GameData> games = await _data.FindAsync(x => x.Id.Equals(gameId));
        return await games.FirstOrDefaultAsync();
    }

    public Task UpdateGame(GameData game) => _data.ReplaceOneAsync(x => x.Id.Equals(game.Id), game);
}