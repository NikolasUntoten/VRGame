using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberBlade : MonoBehaviour {

	[Range(0, 50f)]
	public float Speed = 10f;

	private Vector3 initPos;

	private float size;

	// Use this for initialization
	void Start () {
		initPos = transform.localPosition;
		size = transform.localScale.y;
		transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
	}

	public void Open() {
		StartCoroutine(OpenBlade());
	}

	public void Close() {
		StartCoroutine(CloseBlade());
	}

	IEnumerator OpenBlade() {
		float currentSize = 0.0f;
		SetVisible(true);
		while (currentSize < size) {
			currentSize += .1f;
			transform.localPosition = new Vector3(initPos.x, initPos.y - (size - currentSize), initPos.z);
			transform.localScale = new Vector3(transform.localScale.x, currentSize, transform.localScale.z);
			yield return new WaitForSeconds(1/(Speed*10));
		}
	}

	IEnumerator CloseBlade() {
		float currentSize = size;
		while (currentSize > 0) {
			currentSize -= .1f;
			transform.localPosition = new Vector3(initPos.x, initPos.y - (size - currentSize), initPos.z);
			transform.localScale = new Vector3(transform.localScale.x, currentSize, transform.localScale.z);
			yield return new WaitForSeconds(1 / (Speed * 10));
		}
		SetVisible(false);
	}

	private void SetVisible(bool v) {
		GetComponent<Renderer>().enabled = v;
		Renderer[] arr = gameObject.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in arr) {
			r.enabled = v;
		}
	}
}
