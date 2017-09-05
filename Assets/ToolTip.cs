using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public Sprite[] descriptions;

    public Image img;

    public void Display(Sprite sprite)
    {
        img.sprite = sprite;
        img.color = new Color(img.color.r, img.color.g, img.color.b, sprite == null ? 0 : 255);
    }

    public void DisplayNull()
    {
        img.sprite = null;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
    }

    private void OnGUI()
    {
        if(img.sprite != null)
        {
            transform.localPosition = Event.current.mousePosition;
        }
    }
}
