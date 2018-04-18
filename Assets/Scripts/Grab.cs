using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {

	protected GameObject collidingObject;

	protected GameObject objectInHand;

	protected SteamVR_TrackedObject trackedObj;

	protected bool handVisible;

	protected SteamVR_Controller.Device Controller {
		get {
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		SetControllerVisible(transform.gameObject, true);
	}

	void Update() {
		if (Controller.GetHairTriggerDown()) {
			if (collidingObject) {
				GrabObject();
			}
		}
		if (Controller.GetHairTriggerUp()) {
			if (objectInHand) {
				ReleaseObject();
			}
		}
	}

	virtual protected void GrabObject() {
		objectInHand = collidingObject;
		collidingObject = null;
		var joint = AddFixedJoint();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
	}

	protected FixedJoint AddFixedJoint() {
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	virtual protected void ReleaseObject() {
		if (GetComponent<FixedJoint>()) {
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());

			objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;

			objectInHand = null;
		}

		if (!handVisible) {
			SetControllerVisible(transform.gameObject, true);
		}
	}

	protected void SetCollidingObject(Collider col) {
		if (collidingObject || !col.GetComponent<Rigidbody>()) {
			return;
		}

		collidingObject = col.gameObject;
	}

	public void OnTriggerEnter(Collider other) {
		SetCollidingObject(other);
	}

	public void OnTriggerStay(Collider other) {
		SetCollidingObject(other);
	}

	public void OnTriggerExit(Collider other) {
		collidingObject = null;
	}

	protected void SetControllerVisible(GameObject controller, bool visible) {
		handVisible = visible;
		foreach (SteamVR_RenderModel model in controller.GetComponentsInChildren<SteamVR_RenderModel>()) {
			foreach (var child in model.GetComponentsInChildren<MeshRenderer>())
				child.enabled = visible;
		}
	}
}
