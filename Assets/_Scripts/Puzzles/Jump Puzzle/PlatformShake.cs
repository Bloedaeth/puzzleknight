using UnityEngine;

public class PlatformShake : MonoBehaviour
{
    public float shake_speed;
    public float shake_intensity;
    public Vector3 originPosition;

    public bool isShaking = true;

    private void Start()
    {
        originPosition = transform.position;
    }

    private void Update()
    {
        if(isShaking)
        {
            float step = shake_speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, originPosition + Random.insideUnitSphere, step);
        }
    }
}
