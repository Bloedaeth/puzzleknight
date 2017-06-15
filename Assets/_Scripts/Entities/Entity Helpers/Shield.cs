using UnityEngine;

public class Shield : MonoBehaviour
{
    public Entity Self;

    public bool IsBlocking { get; set; }

    public bool BlockSuccessful()
    {
        //80% chance to block an attack
        return Random.Range(0, 100) < 80;
    }
}
