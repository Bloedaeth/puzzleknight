using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        if(slider.value == slider.maxValue && Input.anyKey)
        {
            FindObjectOfType<LevelManager>().ContinueToScene();
        }
    }
}
