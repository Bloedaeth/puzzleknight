using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private int numEntitiesOnPlate;

    private RaisePlatform platform;
    private bool triggered = false;

    private void Awake()
    {
        platform = FindObjectOfType<RaisePlatform>();
    }

    private void Update()
    {
        if(!triggered)
            platform.PressurePlateActive = false;
        triggered = false;
        //if(numEntitiesOnPlate > 0)
        //{
        //    if(!platform.PressurePlateActive)
        //        platform.PressurePlateActive = true;
        //}
        //else if(platform.PressurePlateActive)
        //    platform.PressurePlateActive = false;
    }

    private void OnTriggerStay(Collider other)
    {
        triggered = true;
        if(!platform.PressurePlateActive)
            platform.PressurePlateActive = true;
    }

    //private void OnTriggerEnter(Collider o)
    //{
    //    ++numEntitiesOnPlate;
    //}

    //private void OnTriggerExit(Collider o)
    //{
    //    --numEntitiesOnPlate;
    //}
}
