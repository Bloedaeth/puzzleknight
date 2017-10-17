using GameLogging;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeathCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("KILLING ENTITY " + other.name);
        BuildDebug.Log("Death collider activated on " + other.name);
        if(other.GetComponent<DeathAnimation>())
        {
            BuildDebug.Log("Death collider ignored due to death animation being present");
            return;
        }

        Health hp = other.GetComponent<Health>();
        if(hp)
            hp.ForceKill();
    }
}
