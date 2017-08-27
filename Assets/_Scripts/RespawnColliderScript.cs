using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnColliderScript : MonoBehaviour
{
    public Transform SpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        Health hp = other.GetComponent<Health>();
        if(hp)
            hp.TakeDamage(hp.InitialAndMaxHealth);
    }
}
