public class GameState
{
    public enum GameMode { Adventure, TimeTrial }
    public enum GameDifficulty { Easy, Normal, Hard }

    public int CurrentGameLevel;
    public int NumLootItemsCollected;
    public int NumEnemiesKilled;

    public Player PlayerState;

    public GameMode Mode;
    public GameDifficulty Difficulty;

    //Used by LitJSON
    public GameState() { }

    public GameState(Player player)
    {
        PlayerState = player;
    }
}
