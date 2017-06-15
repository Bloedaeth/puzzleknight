using UnityEngine;
using System;
using System.IO;
using LitJson;

public class GameStateManager : MonoBehaviour
{
    private string savedGameInfoFile;

    private void Awake()
    {
        savedGameInfoFile = Application.persistentDataPath + "/gameState.json";
        CurrentGameState = new GameState(FindObjectOfType<Player>());
    }

    /// <summary>
    /// The current state of the game. Readonly property, write directly to the GameState class to modify.
    /// </summary>
	public GameState CurrentGameState { get; private set; }

    /// <summary>
    /// Saves the game state.
    /// </summary>
    public void SaveGame()
    {
        try
        {
            File.WriteAllText(savedGameInfoFile, JsonMapper.ToJson(CurrentGameState));
        }
        catch(Exception e)
        {
            Debug.Log("Error saving game: " + e);
        }
    }

    /// <summary>
    /// Loads the game state.
    /// </summary>
    public void LoadGame()
    {
        try
        {
            CurrentGameState = File.Exists(savedGameInfoFile)
                ? JsonMapper.ToObject<GameState>(File.ReadAllText(savedGameInfoFile))
                : new GameState();
        }
        catch (Exception e)
        {
            Debug.Log("Error loading data: " + e);
            CurrentGameState = new GameState(FindObjectOfType<Player>());
        }
    }
}
