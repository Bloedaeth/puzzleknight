using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class CustomAnalytics : MonoBehaviour
{
    public int PotionsBought { get; set; }
    public int PotionsConsumed { get; set; }
    public int CoinsCollected { get; set; }
    public int EnemiesKilled { get; set; }
    public int Deaths { get; set; }
    
    private static CustomAnalytics instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void DoorPieceCollected(string puzzle)
    {
        if(!PlayPrefs.LoggingEnabled)
            return;

        Analytics.CustomEvent(puzzle, new Dictionary<string, object> {
            { "timeTilCompleted", Time.timeSinceLevelLoad }
        });
    }

    public void DoorOpened()
    {
        if(!PlayPrefs.LoggingEnabled)
            return;
        
        Analytics.CustomEvent("doorOpened", new Dictionary<string, object>
        {
            { "potionsBought", PotionsBought },
            { "potionsConsumed", PotionsConsumed },
            { "coinsCollected", CoinsCollected },
            { "enemiesKilled", EnemiesKilled },
            { "deaths", Deaths },
            { "doorOpenTime", Time.timeSinceLevelLoad }
        });
    }

    public void BossDefeated()
    {
        if(!PlayPrefs.LoggingEnabled)
            return;
        
        Analytics.CustomEvent("bossDefeated", new Dictionary <string, object>
        {
            { "potionsBought", PotionsBought },
            { "potionsConsumed", PotionsConsumed },
            { "coinsCollected", CoinsCollected },
            { "enemiesKilled", EnemiesKilled },
            { "deaths", Deaths },
            { "bossBeatTime", Time.timeSinceLevelLoad }
        });
    }
}
