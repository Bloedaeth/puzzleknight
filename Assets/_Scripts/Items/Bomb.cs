public class Bomb : ThrowableItem
{
    public int Damage;
    
    public override void UseOn(Entity self)
    {
        base.UseOn(self);
        DealDamageTo(self);
    }

    protected override void UseOn(Entity[] targets)
    {
        base.UseOn(targets);
        for(int i = 0; i < targets.Length; ++i)
            DealDamageTo(targets[i]);
    }

    private void DealDamageTo(Entity entity)
    {
        entity.GetComponent<Health>().TakeDamage(Damage);
    }
}
