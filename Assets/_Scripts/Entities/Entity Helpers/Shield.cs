using UnityEngine;

public class Shield : MonoBehaviour
{
    public Entity Self;

    /// <summary>Gets whether the entity is currently blocking.</summary>
    public bool IsBlocking { get; set; }

    /// <summary>Performs a random check to determine whether the block was successful or not.</summary>
    /// <returns>true if successful, false otherwise.</returns>
    public bool BlockSuccessful(float angle)
    {
        //80% chance to block an attack
        //return Random.Range(0, 100) < 80;
        return (angle < 90.0f);
    }
}
