using GameLogging;
using UnityEngine;

public class DoorPiece : MonoBehaviour
{
    public enum PieceType { Frame = 0, Panel = 1, Knob = 2 };

    public PieceType Type;
    public AudioClip CollectedClip;

    public PieceCollectNotif popupText;
    public GameObject popupPiece;

    private const int MONEY_REWARD = 50;
    private const float SPEED = 4f;

	bool collected = false;

	void OnEnable() {
		if (collected) {
			gameObject.SetActive(false);
		}
	}

    private void Update()
    {
        transform.eulerAngles += Vector3.up * SPEED;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            BuildDebug.Log("Door Piece collected: " + Type);
            AudioSource.PlayClipAtPoint(CollectedClip, transform.position, PlayPrefs.GameSoundVolume);
            Inventory inventory = other.GetComponent<Inventory>();
            inventory.AddDoorPiece(Type);
            inventory.AddMoney(MONEY_REWARD);
<<<<<<< HEAD
            
            popupText.Activate(popupPiece);

=======
			collected = true;
>>>>>>> Particle_Efficiency_Enhancements
            gameObject.SetActive(false);
        }
    }
}
