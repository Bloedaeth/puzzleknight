using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Sprite Icon;
    public Sprite BlankIcon;
    public AudioClip PickupSound;
    public AudioClip UseSound;

    /// <summary>Plays the use sound of the item.</summary>
    public void PlayUseSound()
    {
        AudioSource.PlayClipAtPoint(UseSound, transform.position);
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
        
        AudioSource.PlayClipAtPoint(PickupSound, transform.position);
        inv.AddItem(this);
    }
}
