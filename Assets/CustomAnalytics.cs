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

    private float timeTilDoorOpened;
    private float timeTilBossDefeated;
    
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

    public void DoorOpened()
    {
        timeTilDoorOpened = Time.timeSinceLevelLoad;
        Analytics.CustomEvent("doorOpened", new Dictionary<string, object>
        {
            { "potionsBought", PotionsBought },
            { "potionsConsumed", PotionsConsumed },
            { "coinsCollected", CoinsCollected },
            { "enemiesKilled", EnemiesKilled },
            { "deaths", Deaths },
            { "doorOpenTime", timeTilDoorOpened }
        });
    }

    public void BossDefeated()
    {
        timeTilBossDefeated = Time.timeSinceLevelLoad;
        Analytics.CustomEvent("bossDefeated", new Dictionary <string, object>
        {
            { "potionsBought", PotionsBought },
            { "potionsConsumed", PotionsConsumed },
            { "coinsCollected", CoinsCollected },
            { "enemiesKilled", EnemiesKilled },
            { "deaths", Deaths },
            { "bossBeatTime", timeTilBossDefeated }
        });

        FindObjectOfType<LevelManager>().LoadNextLevel();
    }
}
