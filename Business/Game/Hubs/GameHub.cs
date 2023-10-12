using Game.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Game.Hubs;

public class GameHub : Hub
{
    private readonly IGameBusiness _gameBusiness;

    private const string __wordPendingEventName = "WordPending";

    private const string __pendingWordRejectedEventName = "PendingWordRejected";

    private const string __pendingWordAcceptedEventName = "PendingWordAccepted";

    private const string __gameClosedEventName = "GameClosed";

    public GameHub(IGameBusiness gameBusiness)
    {
        _gameBusiness = gameBusiness;
    }

    public async Task<object> JoinGame(string gameId, string playerId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

        (List<string> words, string currentPlayerId, string topic) = await _gameBusiness.GetCurrentGameState(gameId, playerId);
        return new
                   {
                       Words = words,
                       CurrentPlayerId = currentPlayerId,
                       Topic = topic
                   };
    }

    public Task LeaveGame(string gameId) => Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);

    public async Task<object> SendPendingWord(string gameId, string playerId, string word)
    {
        (bool accepted, string currentPlayerId) = await _gameBusiness.SendPendingWord(gameId, playerId, word);
        if (accepted)
            _ = Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync(__wordPendingEventName, word, currentPlayerId);
        return new
                   {
                       CallAccepted = accepted,
                       CurrentPlayerId = currentPlayerId
                   };
    }

    public async Task<object> RejectPendingWord(string gameId, string playerId)
    {
        (bool result, string currentPlayerId) = await _gameBusiness.RejectPendingWord(gameId, playerId);
        if (result)
            _ = Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync(__pendingWordRejectedEventName, currentPlayerId);

        return new
                   {
                       CallAccepted = result,
                       CurrentPlayerId = currentPlayerId
                   };
    }

    public async Task<object> AcceptPendingWord(string gameId, string playerId)
    {
        (bool result, string currentPlayerId) = await _gameBusiness.AcceptPendingWord(gameId, playerId);
        if (result)
            _ = Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync(__pendingWordAcceptedEventName, currentPlayerId);
        return new
        {
            CallAccepted = result,
            CurrentPlayerId = currentPlayerId
        };
    }

    public async Task<object> CloseGame(string gameId, string playerId)
    {
        bool result = await _gameBusiness.CloseGame(gameId, playerId);
        if (result)
            _ = Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync(__gameClosedEventName, playerId);
        return result;
    }
}