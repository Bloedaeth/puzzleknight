using UnityEngine;

public class Pylon : MonoBehaviour, IFreezable
{
    private static int numPylonsActive = 0;

    public int ID;
    public bool SlowedTime { get; set; }

    private Lever lever;
    private BossEnemy boss;

    private bool isActive = false;
    private bool scaledBoss = false;

    private float SPEED_MODIFIER = 5f;
    private float MIN_HEIGHT = 470f;
    private float MAX_HEIGHT = 490f;

    private float BOSS_SCALE_MULT = 0.8f;

    private void Start()
    {
        lever = GetComponentInChildren<Lever>();
        boss = FindObjectOfType<BossEnemy>();
    }

    private void Update()
    {
        if(SlowedTime)
            SPEED_MODIFIER = 0.5f;
        else
            SPEED_MODIFIER = 5f;

        if(isActive)
        {
            if(transform.localPosition.y < MAX_HEIGHT)
                transform.localPosition += transform.up * Time.deltaTime * SPEED_MODIFIER;
            else if(!scaledBoss)
            {
                scaledBoss = true;
                lever.SetLeverActive(true);
                boss.BossScaleMult += BOSS_SCALE_MULT;
            }
        }
        else
        {
            if(transform.localPosition.y > MIN_HEIGHT)
                transform.localPosition -= transform.up * Time.deltaTime * SPEED_MODIFIER;
            else if(scaledBoss)
            {
                scaledBoss = false;
                lever.SetLeverActive(false);
                boss.BossScaleMult -= BOSS_SCALE_MULT;

                Invoke("AutoActivate", numPylonsActive > 0 ? 0.5f : 3f);
            }
        }
    }

    private void AutoActivate()
    {
        SetPylonActive(true);
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
