using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Coin : MonoBehaviour
{
    public int CoinValue;
    
    public AudioClip[] pickupSounds;

    private void OnTriggerEnter(Collider other)
    {
        Inventory inv = other.gameObject.GetComponent<Inventory>();
        if(inv == null)
            return;

        inv.AddMoney(CoinValue);
        AudioClip clip = pickupSounds[Random.Range(0, pickupSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, transform.position);
        gameObject.SetActive(false);
    }
}
