using UnityEngine;

public class DoorPiece : MonoBehaviour
{
    public enum PieceType { Frame = 0, Panel = 1, Knob = 2 };

    public PieceType Type;
    public AudioClip CollectedClip;

    private const int MONEY_REWARD = 75;
    private const float SPEED = 2f;

    private void Update()
    {
        transform.eulerAngles += Vector3.up * SPEED;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(CollectedClip, Camera.main.transform.position, 1f);
            Inventory inventory = other.GetComponent<Inventory>();
            inventory.AddDoorPiece(Type);
            inventory.AddMoney(MONEY_REWARD);
            gameObject.SetActive(false);
        }
    }
}
