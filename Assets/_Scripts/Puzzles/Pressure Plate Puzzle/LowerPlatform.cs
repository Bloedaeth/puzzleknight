using UnityEngine;
using GameLogging;

public class LowerPlatform : MonoBehaviour
{
    private bool pressurePlateActive;
    public bool PressurePlateActive
    {
        get { return pressurePlateActive; }
        set
        {
            pressurePlateActive = value;
            BuildDebug.Log("Pressure plate puzzle " + (value ? "de" : "") + "activated");
            Debug.Log(value);
        }
    }

    private new AudioSource audio;

    private const float MAX_HEIGHT = -5.6f;
    private const float MIN_HEIGHT = -10.9f;
    private const float SPEED_MODIFIER = 2f;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(PressurePlateActive)
        {
            if(transform.localPosition.y > MIN_HEIGHT)
            {
                if(!audio.isPlaying)
                    audio.Play();

                transform.localPosition -= transform.up * Time.deltaTime * SPEED_MODIFIER;
            }
            else if(audio.isPlaying)
                audio.Stop();
        }
        else
        {
            if(transform.localPosition.y < MAX_HEIGHT)
            {
                if(!audio.isPlaying)
                    audio.Play();

                transform.localPosition += transform.up * Time.deltaTime * SPEED_MODIFIER;
            }
            else if(audio.isPlaying)
                audio.Stop();
        }
    }
}

