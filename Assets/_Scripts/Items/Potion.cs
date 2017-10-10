using GameLogging;
using UnityEngine;

public class Potion : Item
{
    public override int TypeId { get { return 1; } }

    public override int ShopCost { get { return 50; } }

    [SerializeField] private Sprite inventoryTooltip;
    public override Sprite InventoryTooltip { get { return inventoryTooltip; } }

    [SerializeField] private Sprite shopTooltip;
    public override Sprite ShopTooltip { get { return shopTooltip; } }

    //public enum PotionType { Health, Damage, Speed }

    //public PotionType Type;

    private int healthPotionHealAmount = 25;
    //private int damagePotionDamageBoost = 10;
    //private int speedPotionSpeedBoost = 10;

    /// <summary>Uses the item on the given entity.</summary>
    /// <param name="self">The entity using the item.</param>
    public override void UseOn(Entity self)
    {
        BuildDebug.Log("Potion used");
        PlayUseSound();
        ApplyEffect(self);
    }

    /// <summary>Uses the item on the given list of entities.</summary>
    /// <param name="targets">The list of entities to use the item on.</param>
    protected override void UseOn(Entity[] targets)
    {
        PlayUseSound();
        for(int i = 0; i < targets.Length; ++i)
            ApplyEffect(targets[i]);
    }

    private void ApplyEffect(Entity entity)
    {
        entity.GetComponent<Health>().RecoverHealth(healthPotionHealAmount);
        //switch(Type)
        //{
        //    case PotionType.Health:
        //        entity.GetComponent<Health>().RecoverHealth(healthPotionHealAmount);
        //        break;
        //    case PotionType.Damage:
        //        //Increase damage
        //        break;
        //    case PotionType.Speed:
        //        //Increase speed
        //        break;
        //}
    }
}
