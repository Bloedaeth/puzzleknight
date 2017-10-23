using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoggingPopup : MonoBehaviour
{
    [SerializeField] private GameObject[] menuButtons;

    private void Awake()
    {
        if(PlayPrefs.PopupDisplayed)
            SetupMenu();
    }

    public void ClosePopup()
    {
        PlayPrefs.PopupDisplayed = true;
        PlayPrefs.LoggingEnabled = FindObjectOfType<Toggle>().isOn;
        SetupMenu();
    }

    private void SetupMenu()
    {
        foreach(GameObject btn in menuButtons)
            btn.SetActive(true);

        gameObject.SetActive(false);
    }
}
