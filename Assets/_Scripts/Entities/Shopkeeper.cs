using GameLogging;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Shopkeeper : MonoBehaviour
{
    public AudioClip[] Greetings;
    public AudioClip[] Goodbyes;
    public AudioClip[] Purchases;

    private new AudioSource audio;
    private Animator anim;
    private const float SOUND_COOLDOWN = 10f;

    private float timeSinceGreet = SOUND_COOLDOWN;
    private float timeSinceGoodbye = SOUND_COOLDOWN;

    private int beckonHash;
    private int waveHash;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        beckonHash = Animator.StringToHash("Base Layer.Beckon");
        waveHash = Animator.StringToHash("Base Layer.Wave");
    }

    private void Update()
    {
        timeSinceGreet += Time.deltaTime;
        timeSinceGoodbye += Time.deltaTime;
    }

    public void PlayGreeting()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash != beckonHash)
            anim.SetTrigger("Beckon");

        if(Greetings.Length == 0 || timeSinceGreet < SOUND_COOLDOWN)
            return;

        BuildDebug.Log("Playing Shopkeeper greeting.");
        BuildDebug.Log("# possible sounds: " + Greetings.Length);

        audio.clip = Greetings[Random.Range(0, Greetings.Length)];
        audio.Play();

        timeSinceGreet = 0f;
    }

    public void PlayGoodbye()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash != waveHash)
            anim.SetTrigger("Wave");

        if(Goodbyes.Length == 0 || timeSinceGoodbye < SOUND_COOLDOWN)
            return;

        BuildDebug.Log("Playing Shopkeeper farewell.");
        BuildDebug.Log("# possible sounds: " + Goodbyes.Length);

        audio.clip = Goodbyes[Random.Range(0, Goodbyes.Length)];
        audio.Play();

        timeSinceGoodbye = 0f;
    }

    public void PlayPurchase()
    {
        if(Purchases.Length == 0)
            return;

        BuildDebug.Log("Playing Shopkeeper purchase sound.");
        BuildDebug.Log("# possible sounds: " + Purchases.Length);

        audio.clip = Purchases[Random.Range(0, Purchases.Length)];
        audio.Play();
    }
}
