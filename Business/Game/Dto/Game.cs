using System.Diagnostics;

namespace Game.Dto;

public class GameData
{
    public string Id { get; set; } = "";
    public string PlayerOneId { get; set; } = "";
    public string PlayerTwoId { get; set; } = "";
    public Player CurrentPlayer { get; set; } = Player.PlayerTwo;
    public List<string> Words { get; set; } = new();
    public string? PendingWord { get; set; }
    public string Topic { get; set; }

    public string CurrentPlayerId() =>
        CurrentPlayer switch
        {
            Player.PlayerOne => PlayerOneId,
            Player.PlayerTwo => PlayerTwoId,
        };

    public void SwitchCurrentPlayer() => CurrentPlayer = CurrentPlayer == Player.PlayerOne ? Player.PlayerTwo : Player.PlayerOne;
}


public enum Player
{
    PlayerOne,
    PlayerTwo
}