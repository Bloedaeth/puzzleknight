﻿using UnityEngine.UI;
using UnityEngine;
using GameLogging;

public class InteractScript : MonoBehaviour
{
    [SerializeField] private Image customImage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            BuildDebug.Log("Displaying interact image");
            customImage.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if(FindObjectOfType<Player>().Shopping)
        {
            BuildDebug.Log("Hiding interact image - shopping");
            customImage.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BuildDebug.Log("Hiding interact image - left area");
            customImage.gameObject.SetActive(false);
        }
    }
}
