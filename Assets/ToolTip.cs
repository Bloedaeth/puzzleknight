using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {

	public Sprite[] descriptions;

	public Image img;

	public void Display(Sprite sprite)
	{
		Debug.Log(sprite);
		img.sprite = sprite;
		img.color = new Color(img.color.r, img.color.g, img.color.b, sprite == null ? 0 : 255);
	}
}
