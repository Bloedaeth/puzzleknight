using UnityEngine;

public class LowerPlatform : MonoBehaviour
{
    /// <summary>Is an entity on the pressure plate.</summary>
    public bool PressurePlateActive;

    private const float MAX_HEIGHT = -5.6f;
    private const float MIN_HEIGHT = -10.5f;
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

