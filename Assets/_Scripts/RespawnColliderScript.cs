using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnColliderScript : MonoBehaviour
{
    public Transform SpawnPoint;

    public bool IsPressurePlateRespawn;

    private GameObject crystal;

    private void Awake()
    {
        if(IsPressurePlateRespawn)
            crystal = FindObjectOfType<Crystal>().gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position = SpawnPoint.position;

            if(IsPressurePlateRespawn)
                crystal.SetActive(true);
        }
    }
}
