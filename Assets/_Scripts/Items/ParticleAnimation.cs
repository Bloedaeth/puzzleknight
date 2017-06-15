using UnityEngine;

public class ParticleAnimation : MonoBehaviour
{
    public void DisableAfter(float seconds)
    {
        Invoke("Disable", seconds);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
