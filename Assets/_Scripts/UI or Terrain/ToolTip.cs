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
			float x = Event.current.mousePosition.x - FindObjectOfType<Canvas> ().GetComponent<RectTransform> ().rect.width / 2;
			float y = Event.current.mousePosition.y - FindObjectOfType<Canvas> ().GetComponent<RectTransform> ().rect.height / 2;
			Vector3 toolTipPos = new Vector3(x, -y, transform.localPosition.z);
			
			transform.localPosition = toolTipPos;
		}
    }
}
