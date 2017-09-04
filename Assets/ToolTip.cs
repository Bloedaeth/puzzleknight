using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {

	public Sprite[] descriptions;

	private Image img;

	private void Start()
	{
		img = GetComponent<Image>();
	}

	public void Display(Item item)
	{
		if(item == null)
		{
			img.sprite = null;
			img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
		}
		else
		{
			img.sprite = descriptions[item.TypeId-1];
			img.color = new Color(img.color.r, img.color.g, img.color.b, 255);
		}
	}
}
