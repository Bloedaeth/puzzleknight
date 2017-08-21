using UnityEngine;

public class CollectableDoorPiece : MonoBehaviour
{
    protected void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
