using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShopSounds : MonoBehaviour
{
    public AudioClip[] Greetings;
    public AudioClip[] Goodbyes;
    public AudioClip[] Purchases;

    private new AudioSource audio;

    private const float SOUND_COOLDOWN = 10f;

    private float timeSinceGreet;
    private float timeSinceGoodbye;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        timeSinceGreet += Time.deltaTime;
        timeSinceGoodbye += Time.deltaTime;
    }

    public void PlayGreeting()
    {
        if(Greetings.Length == 0 || timeSinceGreet < SOUND_COOLDOWN)
            return;

        audio.clip = Greetings[Random.Range(0, Greetings.Length)];
        audio.Play();

        timeSinceGreet = 0f;
    }

    public void PlayGoodbye()
    {
        if(Goodbyes.Length == 0 || timeSinceGoodbye < SOUND_COOLDOWN)
            return;

        audio.clip = Goodbyes[Random.Range(0, Goodbyes.Length)];
        audio.Play();

        timeSinceGoodbye = 0f;
    }

    public void PlayPurchase()
    {
        if(Purchases.Length == 0)
            return;

        audio.clip = Purchases[Random.Range(0, Purchases.Length)];
        audio.Play();
    }
}
