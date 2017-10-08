using UnityEngine;
using UnityEngine.UI;

public class PieceCollectNotif : MonoBehaviour
{
    private Text txt;
    private GameObject activeChild;

    private const float SPEED = 20;

    private void Awake()
    {
        txt = GetComponent<Text>();
    }

    private void Update()
    {
        if(activeChild)
            activeChild.transform.localEulerAngles += Vector3.up * SPEED;
    }

    public void Activate(GameObject child)
    {
        activeChild = child;
        activeChild.SetActive(true);
        txt.enabled = true;

        Invoke("Deactivate", 5f);
    }

    private void Deactivate()
    {
        activeChild.SetActive(false);
        activeChild = null;
        txt.enabled = false;
    }
}
