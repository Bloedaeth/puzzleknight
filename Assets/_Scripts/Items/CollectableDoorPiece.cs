using UnityEngine;

public class CollectableDoorPiece : MonoBehaviour
{
    public enum DoorPiece { Frame = 0, Panel = 1, Knob = 2 };

    public DoorPiece PieceType;
    public AudioClip CollectedClip;

    private float speed = 2f;

    private void Update()
    {
        transform.eulerAngles += Vector3.up * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(CollectedClip, Camera.main.transform.position, 1f);
            Inventory inventory = other.GetComponent<Inventory>();
            inventory.AddDoorPiece(PieceType);
            gameObject.SetActive(false);
        }
    }
}
