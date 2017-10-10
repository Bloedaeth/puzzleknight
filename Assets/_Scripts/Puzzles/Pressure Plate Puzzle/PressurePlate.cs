using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private int numEntitiesOnPlate;

    private new AudioSource audio;

    private LowerPlatform platform;
    private bool triggered = false;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        platform = FindObjectOfType<LowerPlatform>();
    }

    private void Update()
    {
        if(!triggered && platform.PressurePlateActive)
            platform.PressurePlateActive = false;
        triggered = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Entity>() == null && other.GetComponent<MovableObject>() == null)
            return;

        triggered = true;
        if(!platform.PressurePlateActive)
            platform.PressurePlateActive = true;
    }

    private void OnTriggerEnter(Collider o)
    {
        audio.Play();
    }

    private void OnTriggerExit(Collider o)
    {
        audio.Play();
    }
}
