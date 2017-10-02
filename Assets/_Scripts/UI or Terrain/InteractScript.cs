using UnityEngine.UI;
using UnityEngine;
using GameLogging;

public class InteractScript : MonoBehaviour
{
    [SerializeField] private Image customImage;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            BuildDebug.Log("Displaying interact image");
            customImage.enabled = true;
        }
    }

    private void Update()
    {
        if(FindObjectOfType<Player>().Shopping)
        {
            BuildDebug.Log("Hiding interact image - shopping");
            customImage.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BuildDebug.Log("Hiding interact image - left area");
            customImage.enabled = false;
        }
    }
}
