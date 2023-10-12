using MongoDB.Driver;

namespace Game.Data;

public static class DataAccessExtensions
{
    public static async Task<T> FirstOrDefaultAsync<T>(this Task<IAsyncCursor<T>> previousAction)
    {
        IAsyncCursor<T> s = await previousAction;
        return await s.FirstOrDefaultAsync();
    } 
}