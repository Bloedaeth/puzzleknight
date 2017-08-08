using UnityEngine;

public class RaisePlatform : MonoBehaviour
{
    /// <summary>Is an entity on the pressure plate.</summary>
    public bool PressurePlateActive;

    private const int MAX_HEIGHT = -1;
    private const int MIN_HEIGHT = -15;
    private const int SPEED_MODIFIER = 3;

    private void Update()
    {
        if(PressurePlateActive)
        {
            if(transform.position.y < MAX_HEIGHT)
                transform.position += transform.up * Time.deltaTime * SPEED_MODIFIER;
        }
        else if(transform.position.y > MIN_HEIGHT)
            transform.position -= transform.up * Time.deltaTime * SPEED_MODIFIER;
    }
}

