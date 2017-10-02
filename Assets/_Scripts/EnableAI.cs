using GameLogging;
using UnityEngine;

public class EnableAI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
            return;

        BuildDebug.Log("Enabling tutorial goblin AI");

        GruntEnemy[] enemies = FindObjectsOfType<GruntEnemy>();
        foreach(GruntEnemy e in enemies)
            e.enabled = true;

        this.enabled = false;
    }
}
