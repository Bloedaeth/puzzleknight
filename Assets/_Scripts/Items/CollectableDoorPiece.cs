using UnityEngine;

public class CollectableDoorPiece : MonoBehaviour
{
    public int DoorPieceID;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
	    other.GetComponent<Inventory>().doorPieces[DoorPieceID-1] = true;
            gameObject.SetActive(false);
        }
    }
}
