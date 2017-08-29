using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InteractScript : MonoBehaviour
{

    [SerializeField] private Image customImage;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            customImage.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            customImage.enabled = false;
        }
    }
}
