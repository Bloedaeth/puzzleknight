using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
            return;

        GruntEnemy[] enemies = FindObjectsOfType<GruntEnemy>();
        foreach(GruntEnemy e in enemies)
            e.enabled = true;

        this.enabled = false;
    }
}
