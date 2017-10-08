using UnityEngine;
using UnityEngine.UI;

public class DoorPiece : MonoBehaviour
{
    public enum PieceType { Frame = 0, Panel = 1, Knob = 2 };

    public PieceType Type;
    public AudioClip CollectedClip;

    public PieceCollectNotif popupText;
    public GameObject popupPiece;

    private const int MONEY_REWARD = 50;
    private const float SPEED = 2f;

    private void Update()
    {
        transform.eulerAngles += Vector3.up * SPEED;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(CollectedClip, transform.position, PlayPrefs.GameSoundVolume);
            Inventory inventory = other.GetComponent<Inventory>();
            inventory.AddDoorPiece(Type);
            inventory.AddMoney(MONEY_REWARD);
            
            popupText.Activate(popupPiece);

            gameObject.SetActive(false);
        }
    }
}
