using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeathCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<DeathAnimation>())
            return;

        Health hp = other.GetComponent<Health>();
        if(hp)
            hp.TakeDamage(hp.InitialAndMaxHealth);
    }
}
