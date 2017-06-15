using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class ThrowableItem : Item
{
    public GameObject CollisionEffect;

    /// <summary>The sound to be played when the item hits something.</summary>
    public AudioClip ThrowHitSound;

    public float ThrowRange;
    public bool IsAoe;
    public float AoeRange;

    protected bool active;
    protected Collider throwHit;

    public override void UseOn(Entity self)
    {
        base.UseOn(self);
    }

    protected override void UseOn(Entity[] targets)
    {
        base.UseOn(targets);
    }

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
            AudioSource.PlayClipAtPoint(ThrowHitSound, transform.position);
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
