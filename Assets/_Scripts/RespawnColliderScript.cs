using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnColliderScript : MonoBehaviour
{
    public Transform SpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player") other.transform.position = SpawnPoint.position;
    }
}
