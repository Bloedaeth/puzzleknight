using System.Collections;
using UnityEngine;

public class SimulateThrow : MonoBehaviour
{
    public GameObject ParticleEffect;
    public Transform Player;
    public float ThrowRange;

    private float gravity = 1f;
    private float launchAngle = 45f;

    /// <summary>Simulates an item being thrown along an arced trajectory.</summary>
    /// <returns>null</returns>
    public IEnumerator ThrowSimulation()
    {
        //TODO make variable landing position
        Vector3 targetPosition = Player.position + Player.forward * ThrowRange;

        // Calculate distance to target
        float target_Distance = Vector3.Distance(transform.position, targetPosition);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * launchAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(launchAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(launchAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);

        float elapse_time = 0;

        while(elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
}
