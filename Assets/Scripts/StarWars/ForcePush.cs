using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePush : MonoBehaviour {

	[Range(0, 180)]
	public float Threshold = 12.5f;

	[Range(100, 500)]
	public float Power = 100f;

	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device Controller {
		get {
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void Update () {
		if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) {
			GameObject[] arr = GameObject.FindGameObjectsWithTag("Forceable");
			foreach (GameObject obj in arr) {
				Vector3 line = obj.transform.position - transform.position;
				line.Normalize();
				Vector3 line2 = transform.forward;
				float angle = Vector3.Angle(line, line2);
				if (angle < Threshold) {
					Rigidbody rb = obj.gameObject.GetComponent<Rigidbody>();
					if (obj != null) {
						rb.AddForce(line * Power / line.magnitude + new Vector3(0, 0.3f, 0));
					}
				}
			}
		}
	}
}
