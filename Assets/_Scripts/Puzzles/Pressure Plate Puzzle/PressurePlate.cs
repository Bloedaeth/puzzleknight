using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private LowerPlatform platform;

    private int numEntitiesOnPlate;
    private bool triggered;

    private void Awake()
    {
        platform = FindObjectOfType<LowerPlatform>();
    }

    private void Update()
    {
        if(triggered && !platform.PressurePlateActive)
            platform.PressurePlateActive = true;
        else if(!triggered && platform.PressurePlateActive)
            platform.PressurePlateActive = false;

        triggered = false;
    }

    private void OnTriggerStay(Collider o)
    {
        if(o.GetComponent<Entity>() != null || o.GetComponent<MovableObject>() != null)
            triggered = true;
    }
}
