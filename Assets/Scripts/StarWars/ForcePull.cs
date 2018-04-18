using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePull : MonoBehaviour {

	[Range(0, 180)]
	public float Threshold = 22.5f;

	[Range(0, 10)]
	public float ForcedThreshold = 4f;

	[Range(0, 100)]
	public float Power = 6.5f;

	[Range(1, 100)]
	public float MaxDist = 10f;

	private List<GameObject> forcedObjects;

	private bool forcing;

	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device Controller {
		get {
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		forcedObjects = new List<GameObject>();
		forcing = false;
	}

	void Update() {
		if (Controller.GetPress(SteamVR_Controller.ButtonMask.Grip)) {
			if (!forcing) {
				GameObject[] arr = GameObject.FindGameObjectsWithTag("Forceable");
				foreach (GameObject obj in arr) {
					Force(obj);
				}
				forcing = true;
			}
		} else {
			StopForce();
		}
		CheckForce();
	}

	private void Force(GameObject obj) {

		float angle = GetAngle(obj);

		if (angle < Threshold) {
			Rigidbody rb = obj.gameObject.GetComponent<Rigidbody>();
			if (rb != null) {
				ApplyForce(obj);
				rb.useGravity = false;
				forcedObjects.Add(obj);
			}
		}
	}

	private void CheckForce() {
		GameObject[] arr = forcedObjects.ToArray();
		for (int i = 0; i < arr.Length; i++) {
			GameObject obj = arr[i];
			if (GetAngle(obj) >= Threshold * ForcedThreshold) {
				Rigidbody rb = obj.gameObject.GetComponent<Rigidbody>();
				if (rb != null) {
					rb.useGravity = true;
					forcedObjects.Remove(obj);
				}
			} else {
				ApplyForce(obj);
			}
		}
	}

	private void StopForce() {
		forcing = false;
		GameObject[] arr = forcedObjects.ToArray();
		for (int i = 0; i < arr.Length; i++) {
			GameObject obj = arr[i];
			Rigidbody rb = obj.gameObject.GetComponent<Rigidbody>();
			if (rb != null) {
				rb.useGravity = true;
				forcedObjects.Remove(obj);
			}
		}	
	}

	private void ApplyForce(GameObject obj) {
		Rigidbody rb = obj.gameObject.GetComponent<Rigidbody>();
		if (rb == null) return;

		Vector2 pos = Controller.GetAxis();
		float desiredPos = ((pos.y + 1) / 2) * MaxDist;

		rb.velocity = (((transform.forward * desiredPos) + transform.position) - obj.transform.position) * Power;
	}

	private float GetAngle(GameObject obj) {
		Vector3 line = obj.transform.position - transform.position;
		line.Normalize();
		Vector3 line2 = transform.forward;
		return Vector3.Angle(line, line2);
	}
}
