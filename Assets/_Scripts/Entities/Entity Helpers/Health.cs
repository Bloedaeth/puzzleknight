using UnityEngine;

public class Health : MonoBehaviour
{
    public GUIBarScript HealthText;

    public float InitialAndMaxHealth = 100f;
    public float HealthRemaining;

    public bool WasAttackedRecently { get { return timeSinceDamageTaken < 0.5f; } }
    public float timeSinceDamageTaken;

    public bool IsInvulnerable;

    private new AudioSource audio;
    private AudioClip[] damageSounds;

    private void Awake()
    {
        HealthRemaining = InitialAndMaxHealth;
        HealthText = GetComponentInChildren<GUIBarScript>();

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

        audio.clip = damageSounds[Random.Range(0, damageSounds.Length)];
        audio.Play();

        if (HealthRemaining <= 0)
        {
            gameObject.AddComponent<DeathAnimation>();
            this.enabled = false;
        }

        if (HealthText)
            HealthText.Value = HealthRemaining / InitialAndMaxHealth;
    }

    /// <summary>Heals the entity, increasing its health.</summary>
    /// <param name="amount">The amount of health to recover.</param>
    public void RecoverHealth(int amount)
    {
        HealthRemaining += amount;
        if (HealthRemaining > InitialAndMaxHealth)
            HealthRemaining = InitialAndMaxHealth;

        if (HealthText)
            HealthText.Value = HealthRemaining / InitialAndMaxHealth;
    }
}
