using UnityEngine;

public class ParticleAnimation : MonoBehaviour
{
    /// <summary>Disables the particle effect after a set time period.</summary>
    /// <param name="seconds">The number of seconds before the effect is disabled.</param>
    public void DisableAfter(float seconds)
    {
        Invoke("Disable", seconds);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
