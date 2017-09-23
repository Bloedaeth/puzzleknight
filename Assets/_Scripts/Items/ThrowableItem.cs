using System.Linq;
using UnityEngine;

public abstract class ThrowableItem : Item
{
    /// <summary>The particle effect to be displayed when the item collides with an object.</summary>
    public GameObject CollisionEffect;

    /// <summary>The sound to be played when the item hits something.</summary>
    public AudioClip ThrowHitSound;

    public float ThrowRange;
    public bool IsAoe;
    public float AoeRange;

    protected bool active;
    protected Collider throwHit;

    /// <summary>Uses the item on the given entity.</summary>
    /// <param name="self">The entity using the item.</param>
    public abstract override void UseOn(Entity self);

    /// <summary>Uses the item on the given list of entities.</summary>
    /// <param name="targets">The list of entities to use the item on.</param>
    protected abstract override void UseOn(Entity[] targets);

    /// <summary>Throws the item in the direction the entity is facing.</summary>
    public virtual void Throw()
    {
        SimulateThrow throwSim = gameObject.AddComponent<SimulateThrow>();
        active = true;

        Transform player = FindObjectOfType<Player>().transform;
        throwSim.Player = player.transform;
        throwSim.ThrowRange = ThrowRange;

        transform.position = player.position + new Vector3(0f, 1f, 0f) + player.forward;
        gameObject.SetActive(true);

        StartCoroutine(throwSim.ThrowSimulation());
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(active)
        {
            AudioSource.PlayClipAtPoint(ThrowHitSound, transform.position, PlayPrefs.GameSoundVolume);
            StopAllCoroutines();
            Entity[] hitEntities = GetHitEntities();
            if(hitEntities != null)
                UseOn(hitEntities);

            GameObject particle = Instantiate(CollisionEffect, transform.position, Quaternion.identity) as GameObject;
            particle.GetComponent<ParticleAnimation>().DisableAfter(1.5f);
            gameObject.SetActive(false);
        }
        else
            base.OnTriggerEnter(other);
    }

    private Entity[] GetHitEntities()
    {
        if(IsAoe)
        {
            Entity[] targets = Physics.OverlapSphere(transform.position, AoeRange)
                               .Select(col => col.GetComponent<Entity>())
                               .Where(e => e != null)
                               .ToArray();

            return targets.Length > 0 ? targets : null;
        }
        Entity target = throwHit.GetComponent<Entity>();

        return target ? new Entity[] { target } : null;
    }
}
