using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Sprite Icon;
    public Sprite BlankIcon;
    public AudioClip PickupSound;
    public AudioClip UseSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = FindObjectOfType<Player>().GetComponent<AudioSource>();
    }

    /// <summary>Plays the use sound of the item.</summary>
    public void PlayUseSound()
    {
        audioSource.clip = UseSound;
        audioSource.Play();
        //AudioSource.PlayClipAtPoint(UseSound, transform.position);
    }

    /// <summary>Uses the item on the given entity.</summary>
    /// <param name="self">The entity using the item.</param>
    public abstract void UseOn(Entity self);

    /// <summary>Uses the item on the given list of entities.</summary>
    /// <param name="targets">The list of entities to use the item on.</param>
    protected abstract void UseOn(Entity[] targets);

    protected virtual void OnTriggerEnter(Collider other)
    {
        Inventory inv = other.gameObject.GetComponent<Inventory>();
        if(inv == null)
            return;

        audioSource.clip = PickupSound;
        audioSource.Play();
        //AudioSource.PlayClipAtPoint(PickupSound, transform.position);
        inv.AddItem(this);
    }
}
