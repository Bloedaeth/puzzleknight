using UnityEngine;

public class Health : MonoBehaviour
{
    public GUIBarScript HealthText;

    public float InitialAndMaxHealth = 100f;
    public float HealthRemaining;

    public bool WasAttackedRecently { get { return timeSinceDamageTaken < 0.5f; } }
    private float timeSinceDamageTaken;

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

    public void TakeDamage(int amount)
    {
        timeSinceDamageTaken = 0f;

        HealthRemaining -= amount;

        audio.clip = damageSounds[Random.Range(0, damageSounds.Length)];
        audio.Play();

        if(HealthRemaining <= 0)
        {
            gameObject.AddComponent<DeathAnimation>();
            this.enabled = false;
        }

        if(HealthText)
            HealthText.Value = HealthRemaining / InitialAndMaxHealth;
    }

    public void RecoverHealth(int amount)
    {
        HealthRemaining += amount;
        if(HealthRemaining > InitialAndMaxHealth)
            HealthRemaining = InitialAndMaxHealth;

        if(HealthText)
            HealthText.Value = HealthRemaining / InitialAndMaxHealth;
    }
}
