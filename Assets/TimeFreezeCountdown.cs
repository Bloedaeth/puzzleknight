using UnityEngine;
using UnityEngine.UI;

public class TimeFreezeCountdown : MonoBehaviour
{
    /// <summary>The length of time that the freeze lasts.</summary>
    public float TimerStart;

    private Text text;
    private float timer;
    private const string TIMER_MSG = "Freeze Time Remaining: ";

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        text.text = TIMER_MSG + string.Format("{0:#.##}", TimerStart - timer);
        if(timer >= TimerStart)
            gameObject.SetActive(false);
    }
}
