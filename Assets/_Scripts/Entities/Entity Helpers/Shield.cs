using GameLogging;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Entity Self;

    /// <summary>Gets whether the entity is currently blocking.</summary>
    public bool IsBlocking { get; set; }

    /// <summary>Checks if the angle of attack is within blocking range.</summary>
    /// <returns>true if in range, false otherwise.</returns>
    public bool BlockSuccessful(float angle)
    {
        BuildDebug.Log("Blocking attack");
        return (angle < 90.0f);
    }
}
