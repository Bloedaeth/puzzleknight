using UnityEngine;

public class Health : MonoBehaviour
{
    public int InitialAndMaxHealth = 100;
    public int HealthRemaining;

    public float timeSinceDamageTaken;
    private bool WasAttackedRecently { get { return timeSinceDamageTaken < 0.5f; } }

    public bool IsInvulnerable;

    private new AudioSource audio;

    public GUIHealthBar HealthBar;

    private void Awake()
    {
        HealthRemaining = InitialAndMaxHealth;

        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        timeSinceDamageTaken += Time.deltaTime;
    }

    private void SetHealth(int newHp)
    {
        if(HealthRemaining > newHp)
            timeSinceDamageTaken = 0f;

        HealthRemaining = newHp;
        if(HealthBar)
            HealthBar.UpdateGUI(HealthRemaining, InitialAndMaxHealth);

        if(HealthRemaining == 0)
        {
            gameObject.AddComponent<DeathAnimation>();
            enabled = false;
        }
    }

    /// <summary>Deals damage to the entity, reducing its health.</summary>
    /// <param name="amount">The amount of damage to deal to the entity.</param>
    public void TakeDamage(int amount)
    {
        if(WasAttackedRecently)
            return;

        if(!audio.isPlaying)
        {
            AudioClip[] damageSounds = GetComponent<EntitySoundsCommon>().hurtSounds;
            audio.clip = damageSounds[Random.Range(0, damageSounds.Length)];
            audio.Play();
        }

        if(IsInvulnerable)
            return;

        SetHealth(Mathf.Clamp(HealthRemaining - amount, 0, InitialAndMaxHealth));
    }

    /// <summary>Forcibly kills the entity, regardless of whether it is invulnerable or has been attacked recently.</summary>
    public void ForceKill()
    {
        if(!audio.isPlaying)
        {
            AudioClip[] damageSounds = GetComponent<EntitySoundsCommon>().hurtSounds;
            audio.clip = damageSounds[Random.Range(0, damageSounds.Length)];
            audio.Play();
        }

        SetHealth(0);
    }

    /// <summary>Heals the entity, increasing its health.</summary>
    /// <param name="amount">The amount of health to recover.</param>
    public void RecoverHealth(int amount)
    {
        SetHealth(Mathf.Clamp(HealthRemaining + amount, 0, InitialAndMaxHealth));
    }

    /// <summary>Resets the entity's health to full.</summary>
    public void ResetHealth()
    {
        HealthRemaining = InitialAndMaxHealth;
        if(HealthBar)
            HealthBar.UpdateGUI(HealthRemaining, InitialAndMaxHealth);
    }
}
