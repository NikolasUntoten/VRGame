using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronManEffect : MonoBehaviour {

	public GameObject Particles;

	public void OnTriggerEnter(Collider other) {
		StartCoroutine(Poof());
	}

	IEnumerator Poof() {
		Particles.SetActive(true);
		yield return new WaitForSeconds(.5f);
		Particles.SetActive(false);
	}
}
