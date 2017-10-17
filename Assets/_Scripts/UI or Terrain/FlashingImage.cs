using UnityEngine;
using UnityEngine.UI;

public class FlashingImage : MonoBehaviour
{
    private RawImage img;
    private float nextChangeTime = 0;

    private void OnEnable()
    {
        img = GetComponent<RawImage>();
    }

    private void Update()
    {
        if(Time.time > nextChangeTime)
        {
            img.enabled = !img.enabled;
            nextChangeTime = Time.time + 1f;
        }
    }
}
