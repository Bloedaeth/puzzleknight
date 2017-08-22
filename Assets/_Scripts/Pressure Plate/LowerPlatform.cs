using UnityEngine;

public class LowerPlatform : MonoBehaviour
{
    /// <summary>Is an entity on the pressure plate.</summary>
    public bool PressurePlateActive;

    private const int MAX_HEIGHT = -3;
    private const int MIN_HEIGHT = -8;
    private const int SPEED_MODIFIER = 1;

    private void Update()
    {
        if(PressurePlateActive)
        {
            if(transform.localPosition.y > MIN_HEIGHT)
                transform.localPosition -= transform.up * Time.deltaTime * SPEED_MODIFIER;
        }
        else if(transform.localPosition.y < MAX_HEIGHT)
            transform.localPosition += transform.up * Time.deltaTime * SPEED_MODIFIER;

    }
}

