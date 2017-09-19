using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class RespawnGoblin : MonoBehaviour
{
    public GameObject goblin;

    private bool respawning;

    private void Update()
    {
		if(!goblin.activeInHierarchy && !respawning)
        {
            respawning = true;
            Invoke("Respawn", 2f);
        }
	}

    private void Respawn()
    {
        Destroy(goblin.GetComponent<DeathAnimation>());
        goblin.GetComponent<AICharacterControl>().enabled = true;
        Health hp = goblin.GetComponent<Health>();
        hp.enabled = true;
        hp.HealthRemaining = hp.InitialAndMaxHealth;
        goblin.GetComponent<Animator>().SetTrigger("Respawn");
        goblin.transform.position = transform.position;
        goblin.SetActive(true);
        respawning = false;
    }
}
