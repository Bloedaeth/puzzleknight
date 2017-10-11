using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private int numEntitiesOnPlate;

    private LowerPlatform platform;
    private bool triggered = false;

    private void Awake()
    {
        platform = FindObjectOfType<LowerPlatform>();
    }

    private void Update()
    {
        if(numEntitiesOnPlate == 0 && platform.PressurePlateActive)
            platform.PressurePlateActive = false;
        else if(numEntitiesOnPlate > 0 && !platform.PressurePlateActive)
            platform.PressurePlateActive = true;
    }

    private void OnTriggerEnter(Collider o)
    {
        if(o.GetComponent<Entity>() == null && o.GetComponent<MovableObject>() == null)
            return;

        ++numEntitiesOnPlate;
    }

    private void OnTriggerExit(Collider o)
    {
        if(o.GetComponent<Entity>() == null && o.GetComponent<MovableObject>() == null)
            return;

        --numEntitiesOnPlate;
    }
}
