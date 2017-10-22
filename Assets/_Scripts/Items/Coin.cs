using GameLogging;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Coin : MonoBehaviour
{
    /// <summary>The amount of money that should be given to the entity that collects the coin.</summary>
    public int CoinValue;
    
    /// <summary>The list of sounds to be randomly played when the coin is picked up.</summary>
    public AudioClip[] pickupSounds;

	bool collected = false;

	void OnEnable() {
		if (collected) {
			gameObject.SetActive (false);
		}
	}

    private void OnTriggerEnter(Collider other)
    {
        Inventory inv = other.gameObject.GetComponent<Inventory>();
        if(inv == null)
            return;

        BuildDebug.Log("Coin collected");
        inv.AddMoney(CoinValue);
        AudioClip clip = pickupSounds[Random.Range(0, pickupSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, transform.position, PlayPrefs.GameSoundVolume);
		collected = true;
		gameObject.SetActive (false);

        CustomAnalytics analytics = FindObjectOfType<CustomAnalytics>();
        if(analytics)
            ++analytics.CoinsCollected;
    }
}
