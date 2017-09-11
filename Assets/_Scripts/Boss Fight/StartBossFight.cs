﻿using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class StartBossFight : MonoBehaviour
{
    public BossEnemy boss;
    public GameObject rocks;

    private void OnEnable()
    {
        rocks.SetActive(false);
    }

    private void OnTriggerEnter(Collider col)
    {
        Player p = col.GetComponent<Player>();
        if(!p)
            return;

        p.InBossFight = true;

        boss.GetComponent<AICharacterControl>().SetTarget(col.gameObject.transform);
        boss.GetComponent<BossEnemy>().Stage = 1;

        rocks.SetActive(true);
        gameObject.SetActive(false);
    }
}