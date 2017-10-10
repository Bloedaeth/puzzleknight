using UnityEngine;
using UnityEngine.UI;

public class FlashingText : MonoBehaviour
{
    private Text text;
    private float nextChangeTime = 0;

    private const string EMPTY = "";
    private const string MSG = "Press any key to continue";

    private void OnEnable()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if(Time.time > nextChangeTime)
        {
            text.text = text.text == EMPTY ? MSG : EMPTY;
            nextChangeTime = Time.time + 1f;
        }
    }
}
