using UnityEngine;

public class CollectableDoorPiece : MonoBehaviour
{
    public int DoorPieceID;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
