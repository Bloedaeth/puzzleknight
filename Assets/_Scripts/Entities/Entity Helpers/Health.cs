using UnityEngine;

public class Health : MonoBehaviour
{
    public int InitialAndMaxHealth = 100;
    public int HealthRemaining;

    public bool WasAttackedRecently { get { return timeSinceDamageTaken < 0.5f; } }
    public float timeSinceDamageTaken;

    public bool IsInvulnerable;

    private new AudioSource audio;
    private AudioClip[] damageSounds;

    public GUIHealthBar HealthBar;

    private void Awake()
    {
        HealthRemaining = InitialAndMaxHealth;

        audio = GetComponent<AudioSource>();
        damageSounds = GetComponent<EntitySoundsCommon>().hurtSounds;
    }

    private void Update()
    {
        timeSinceDamageTaken += Time.deltaTime;
    }

    /// <summary>Deals damage to the entity, reducing its health.</summary>
    /// <param name="amount">The amount of damage to deal to the entity.</param>
    public void TakeDamage(int amount)
    {
        timeSinceDamageTaken = 0f;

        HealthRemaining -= amount;
        if(HealthRemaining < 0)
            HealthRemaining = 0;

        audio.clip = damageSounds[Random.Range(0, damageSounds.Length)];
        audio.Play();

        if(HealthRemaining <= 0)
        {
            gameObject.AddComponent<DeathAnimation>();
            enabled = false;
        }

        if(HealthBar)
            HealthBar.UpdateGUI(HealthRemaining, InitialAndMaxHealth);
    }

    /// <summary>Heals the entity, increasing its health.</summary>
    /// <param name="amount">The amount of health to recover.</param>
    public void RecoverHealth(int amount)
    {
        HealthRemaining += amount;
        if (HealthRemaining > InitialAndMaxHealth)
            HealthRemaining = InitialAndMaxHealth;

        if(HealthBar)
            HealthBar.UpdateGUI(HealthRemaining, InitialAndMaxHealth);
    }

    public void ResetHealth()
    {
        HealthRemaining = InitialAndMaxHealth;

        if(HealthBar)
            HealthBar.UpdateGUI(HealthRemaining, InitialAndMaxHealth);
    }
}
