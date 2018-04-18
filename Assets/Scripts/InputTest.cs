using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour {

	public GameObject Ray;

	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device Controller {
		get {
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void Start() {
		if (Ray == null) {
			Debug.Log("error, shit aint working");
			Ray = Instantiate<GameObject>(Ray);
		}
	}
	
	void Update() {
		//float value = Controller.GetAxis().x;
		float dist = 20;
		Ray.transform.localScale = new Vector3(0.01f, 0.01f, dist);
		if (Controller.GetHairTrigger()) {
			Ray.SetActive(true);
			Ray.transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * dist, .5f);
			Ray.transform.LookAt(transform.position + transform.forward*dist);
		} else {
			Ray.SetActive(false);
		}
	}
}
