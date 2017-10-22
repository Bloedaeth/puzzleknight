using GameLogging;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class StartBossFight : MonoBehaviour
{
    public GameObject rocks;
    public GameObject[] potions;

    private BossEnemy boss;

    private void OnEnable()
    {
        boss = FindObjectOfType<BossEnemy>();
        rocks.SetActive(false);
        foreach(GameObject p in potions)
        {
            Item i = p.GetComponent<Item>();
            if(i.collected)
            {
                i.collected = false;
                p.SetActive(true);
                FindObjectOfType<Inventory>().RemoveItem(i);
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Player p = col.GetComponent<Player>();
        if(!p)
            return;

        BuildDebug.Log("Boss fight activated");

        p.InBossFight = true;

        boss.GetComponent<AICharacterControl>().SetTarget(col.gameObject.transform);
        boss.GetComponent<BossEnemy>().Stage = 1;
        boss.GetComponent<Health>().HealthBar.transform.parent.gameObject.SetActive(true);

        rocks.SetActive(true);
        gameObject.SetActive(false);
    }
}
