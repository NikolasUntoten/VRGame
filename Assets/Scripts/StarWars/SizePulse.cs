using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePulse : MonoBehaviour {

	[Range(1, 100)]
	public float Rate;

	private float initSize;

	void Start() {
		initSize = (transform.localScale.x + transform.localScale.z) / 2;
	}

	void Update() {
		float t = Time.time * Rate;
		float s = Mathf.Sin(5 * t) + Mathf.Sin(10 * t) + Mathf.Sin(3 * t + 2) + Mathf.Sin(7 * t + 5);
		s /= 16;
		s += 1;
		float sFinal = initSize * s;
		transform.localScale = new Vector3(sFinal, transform.localScale.y, sFinal);
	}
}
