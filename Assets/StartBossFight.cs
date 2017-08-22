using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class StartBossFight : MonoBehaviour
{
    public BossEnemy boss;

    private void OnTriggerEnter(Collider col)
    {
        if(!col.CompareTag("Player"))
            return;

        boss.GetComponent<AICharacterControl>().SetTarget(col.gameObject.transform);
        boss.GetComponent<BossEnemy>().Stage = 1;
        gameObject.SetActive(false);
    }
}
