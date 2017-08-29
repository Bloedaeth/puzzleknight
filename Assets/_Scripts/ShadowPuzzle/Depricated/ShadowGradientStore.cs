using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowGradientStore : MonoBehaviour {

	Gradient shadow;
	Gradient evilShadow;

	GradientColorKey[][] gcks;
	GradientAlphaKey[] gak;

	public Color startShadowColor;
	public Color normalShadowColor;
	public Color evilShadowColor;

	// Use this for initialization
	void Start () {
		resetGradients ();

	}

	void resetGradients() {
		if (startShadowColor == null) {
			startShadowColor = new Color (150, 0, 80);
		}

		if (normalShadowColor == null) {
			normalShadowColor = new Color (100, 0, 140);
		}

		if (evilShadowColor == null) {
			evilShadowColor = new Color (140, 0, 0);
		}

		gcks = new GradientColorKey[2][];

		for (int i = 0; i < 2; i++) {
			gcks [i] = new GradientColorKey[2];
		}

		gak = new GradientAlphaKey[2];

		gcks [0] [0].color = startShadowColor;
		gcks [1] [0].color = startShadowColor;
		gcks [0] [1].color = normalShadowColor;
		gcks [1] [1].color = evilShadowColor;

		for (int i = 0; i < gcks [0].Length; i++) {
			gcks [i] [0].time = 0f;
			gcks [i] [1].time = 0.5f;
		}

		gak [0].alpha = 1f;
		gak [0].time = 0.25f;
		gak [1].alpha = 0f;
		gak [1].time = 1f;

		shadow = new Gradient ();
		evilShadow = new Gradient ();

		shadow.SetKeys (gcks [0], gak);
		evilShadow.SetKeys (gcks [1], gak);
	}

	Gradient resetGradients(int i) {
		resetGradients();

		if (i == 0) {
			return shadow;
		}

		if (i == 1) {
			return evilShadow;
		}

		return null;
	}

	public Gradient GetShadowGradient() {
		if (shadow == null) {
			return resetGradients (0);
		}
		return shadow;
	}

	public Gradient GetEvilGradient() {
		if (evilShadow == null) {
			return resetGradients (1);
		}
		return evilShadow;
	}
}
