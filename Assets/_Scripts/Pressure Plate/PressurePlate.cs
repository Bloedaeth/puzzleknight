using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private int numEntitiesOnPlate;

    private RaisePlatform platform;

    private void Awake()
    {
        platform = FindObjectOfType<RaisePlatform>();
    }

    private void Update()
    {
        if(numEntitiesOnPlate > 0)
        {
            if(!platform.PressurePlateActive)
                platform.PressurePlateActive = true;
        }
        else if(platform.PressurePlateActive)
            platform.PressurePlateActive = false;
    }

    private void OnTriggerEnter(Collider o)
    {
        ++numEntitiesOnPlate;
    }

    private void OnTriggerExit(Collider o)
    {
        --numEntitiesOnPlate;
    }
}
