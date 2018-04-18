using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JediGrab : Grab {

	override protected void GrabObject() {
		if (collidingObject.GetComponent<Saber>()) {
			collidingObject.GetComponent<Saber>().OpenBlade();
			GrabSaber();
		} else {
			base.GrabObject();
		}
	}

	private void GrabSaber() {
		collidingObject.transform.rotation = trackedObj.transform.Find("Model").rotation;
		collidingObject.transform.Rotate(new Vector3(85, 0, 0));

		collidingObject.transform.position = trackedObj.transform.position;
		collidingObject.transform.position += collidingObject.transform.forward * 0.035f + collidingObject.transform.up * -0.25f;

		SetControllerVisible(transform.gameObject, false);
		base.GrabObject();
	}

	override protected void ReleaseObject() {
		if (objectInHand.GetComponent<Saber>()) {
			objectInHand.GetComponent<Saber>().CloseBlade();
		}
		base.ReleaseObject();
	}
}
