public class Potion : ThrowableItem
{
    public enum PotionType { Health, Damage, Speed }

    public PotionType Type;

    private int healthPotionHealAmount = 25;
    private int damagePotionDamageBoost = 10;
    private int speedPotionSpeedBoost = 10;

    public override void UseOn(Entity self)
    {
        base.UseOn(self);
        ApplyEffect(self);
    }

    protected override void UseOn(Entity[] targets)
    {
        base.UseOn(targets);
        for(int i = 0; i < targets.Length; ++i)
            ApplyEffect(targets[i]);
    }

    private void ApplyEffect(Entity entity)
    {
        switch(Type)
        {
            case PotionType.Health:
                entity.GetComponent<Health>().RecoverHealth(healthPotionHealAmount);
                break;
            case PotionType.Damage:
                //Increase damage
                break;
            case PotionType.Speed:
                //Increase speed
                break;
        }
    }
}
