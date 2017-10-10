using GameLogging;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            BuildDebug.Log("Player standing on platform: " + name, true);
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            BuildDebug.Log("Player no longer on platform", true);
            other.transform.parent = null;
        }
    }
}
