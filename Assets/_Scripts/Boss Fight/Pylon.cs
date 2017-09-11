using UnityEngine;

public class Pylon : MonoBehaviour, IFreezable
{
    private static int numPylonsActive = 0;

    public int ID;
    public bool SlowedTime { get; set; }

    private Lever lever;
    private BossEnemy boss;
    private ParticleSystem ps;

    private bool isActive = false;
    private bool scaledBoss = false;

    private float speedModifier = 5f;

    private const float MIN_HEIGHT = 456f;
    private const float MAX_HEIGHT = 480.5f;
    private const float BOSS_SCALE_MULT = 0.8f;

    private void Start()
    {
        lever = transform.parent.GetComponentInChildren<Lever>();
        boss = FindObjectOfType<BossEnemy>();
        ps = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if(SlowedTime)
            speedModifier = 0.5f;
        else
            speedModifier = 5f;

        if(isActive)
        {
            if(transform.localPosition.y < MAX_HEIGHT)
                transform.localPosition += transform.up * Time.deltaTime * speedModifier;
            else if(!scaledBoss)
            {
                lever.SetLeverActive(true);

                ps.Play();
                scaledBoss = true;
                boss.BossScaleMult += BOSS_SCALE_MULT;
            }
        }
        else
        {
            if(transform.localPosition.y > MIN_HEIGHT)
                transform.localPosition -= transform.up * Time.deltaTime * speedModifier;
            else if(scaledBoss)
            {
                ps.Stop();
                scaledBoss = false;
                boss.BossScaleMult -= BOSS_SCALE_MULT;

                Invoke("AutoActivate", numPylonsActive > 0 ? 0.5f : 3f);
            }
        }
    }

    private void AutoActivate()
    {
        SetPylonActive(true);
    }

    public void ResetPylon()
    {
        if(isActive)
        {
            ps.Stop();
            isActive = false;
            scaledBoss = false;
            --numPylonsActive;
        }
    }

    public void SetPylonActive(bool val)
    {
        isActive = val;
        if(val)
            ++numPylonsActive;
        else
            --numPylonsActive;
    }
}
