using GameLogging;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    /// <summary>The unique ID for each item subclass (used in Inventory.cs).</summary>
    public abstract int TypeId { get; }

    /// <summary>The cost of the item when sold in the shop.</summary>
    public abstract int ShopCost { get; }

    /// <summary>The tooltip to display when the pointer hovers over the item in the inventory.</summary>
    public abstract Sprite InventoryTooltip { get; }
    
    /// <summary>The tooltip to display when the pointer hovers over the item in the shop.</summary>
    public abstract Sprite ShopTooltip { get; }
    
    public Sprite Icon;
    public Sprite BlankIcon;
    public AudioClip PickupSound;
    public AudioClip UseSound;

	public bool collected = false;

    //Shop items are never active so Awake() wasn't called to assign audioSource
    //This method solves that problem so all items (shop/collect) work properly
    //private AudioSource _audioSource;
    //private AudioSource audioSource
    //{
    //    get
    //    {
    //        if(_audioSource == null)
    //            _audioSource = FindObjectOfType<Player>().GetComponent<AudioSource>();
    //        return _audioSource;
    //    }
    //}

	void OnEnable() {
		if (collected) {
			gameObject.SetActive (false);
		}
	}

    /// <summary>Plays the use sound of the item.</summary>
    public void PlayUseSound()
    {
        //audioSource.clip = UseSound;
        //audioSource.Play();
        AudioSource.PlayClipAtPoint(UseSound, FindObjectOfType<Player>().transform.position, PlayPrefs.GameSoundVolume);
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
        if(inv && inv.AddItem(this))
        {
            BuildDebug.Log("Potion collected");
            //audioSource.clip = PickupSound;
            //audioSource.Play();
            AudioSource.PlayClipAtPoint(PickupSound, transform.position, PlayPrefs.GameSoundVolume);
        }
    }
}
