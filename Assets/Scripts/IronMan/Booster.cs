using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {

	public GameObject Rig;

	[Range(0, 100)]
	public float Power = 5f;

	[Range(0, 100)]
	public float FootPower = 2f;

	[Range(0, 5)]
	public float TiltPower = 0.5f;

	private bool FootOn;

	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device Controller {
		get {
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		if (Rig == null) {
			Rig = GameObject.Find("[CameraRig]");
		}
		FootOn = false;
	}

	void Update() {
		if (Controller.GetHairTrigger()) {
			Rig.GetComponent<Rigidbody>().AddForce(transform.forward * Power * -1);
		}

		if (FootOn) {
			Rig.GetComponent<Rigidbody>().AddForce(Rig.transform.up*FootPower);
		}

		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
			FootOn = !FootOn;
		}
	}
}
