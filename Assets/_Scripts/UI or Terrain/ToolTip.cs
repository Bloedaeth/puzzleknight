using GameLogging;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public Sprite[] descriptions;

    public Image img;

    public void Display(Sprite sprite)
    {
        BuildDebug.Log("Tooltip display: " + sprite);
        img.sprite = sprite;
        img.color = new Color(img.color.r, img.color.g, img.color.b, sprite == null ? 0 : 255);
    }

    public void DisplayNull()
    {
        BuildDebug.Log("Tooltip display: null");
        img.sprite = null;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
    }

    private void OnGUI()
	{
		if(img.sprite != null)
        {
            BuildDebug.Log("TOOLTIP POSITION");

            float mouseX = Event.current.mousePosition.x;
            float mouseY = Event.current.mousePosition.y;
            float x = mouseX - FindObjectOfType<Canvas> ().GetComponent<RectTransform> ().rect.width / 2;
			float y = mouseY - FindObjectOfType<Canvas> ().GetComponent<RectTransform> ().rect.height / 2;
			Vector3 toolTipPos = new Vector3(x, -y, transform.localPosition.z);

            BuildDebug.Log("Mouse: " + new Vector2(mouseX, mouseY).ToString(), true);
            BuildDebug.Log("Tooltip: " + new Vector2(x, -y).ToString(), true);

            transform.localPosition = toolTipPos;
		}
    }
}
