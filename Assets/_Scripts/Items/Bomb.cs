public class Bomb : ThrowableItem
{
    public int Damage;

    /// <summary>Uses the item on the given entity.</summary>
    /// <param name="self">The entity using the item.</param>
    public override void UseOn(Entity self)
    {
        PlayUseSound();
        DealDamageTo(self);
    }

    /// <summary>Uses the item on the given list of entities.</summary>
    /// <param name="targets">The list of entities to use the item on.</param>
    protected override void UseOn(Entity[] targets)
    {
        PlayUseSound();
        for(int i = 0; i < targets.Length; ++i)
            DealDamageTo(targets[i]);
    }

    private void DealDamageTo(Entity entity)
    {
        entity.GetComponent<Health>().TakeDamage(Damage);
    }
}
