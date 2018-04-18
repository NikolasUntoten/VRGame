using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : MonoBehaviour {

	public void OpenBlade() {
		gameObject.GetComponentInChildren<SaberBlade>().Open();
	}

	public void CloseBlade() {
		gameObject.GetComponentInChildren<SaberBlade>().Close();
	}
}
