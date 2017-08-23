using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollectableDoorPiece : MonoBehaviour
{
    public enum DoorPiece { Frame = 0, Panel = 1, Knob = 2 };

    public DoorPiece PieceType;

    private float speed = 2f;

    private void Update()
    {
        transform.eulerAngles += Vector3.up * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GetComponent<AudioSource>().Play();
            Inventory inventory = other.GetComponent<Inventory>();
            inventory.AddDoorPiece(PieceType);
            gameObject.SetActive(false);
        }
    }
}
