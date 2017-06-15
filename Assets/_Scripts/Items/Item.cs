using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Sprite Icon;
    public Sprite BlankIcon;
    public AudioClip PickupSound;
    public AudioClip UseSound;

    public virtual void UseOn(Entity self)
    {
        AudioSource.PlayClipAtPoint(UseSound, transform.position);
    }

    protected virtual void UseOn(Entity[] targets)
    {
        AudioSource.PlayClipAtPoint(UseSound, transform.position);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Inventory inv = other.gameObject.GetComponent<Inventory>();
        if(inv == null)
            return;
        
        AudioSource.PlayClipAtPoint(PickupSound, transform.position);
        inv.AddItem(this);
    }
}
