using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shine : MonoBehaviour {

	Vector3 originalPos;
	public float distance = 0.1f;
	public Transform mirrorParent;
	public bool checkRotation = false;
	public Vector3 focus = Vector3.up * 10f;
	public Transform focusOn;

	// Use this for initialization
	void Start () {
		originalPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		var p = focusOn ? focusOn.position : focus;
		var direction = (p - transform.position).normalized;
		direction.z = originalPos.z;
		direction.x = mirrorParent ? mirrorParent.localScale.x * direction.x : direction.x;

		if (checkRotation) {
			var t = transform;
			var parent = t.parent;
			var angle = parent.rotation.eulerAngles.z;
			var aMod = Mathf.Sign (parent.lossyScale.x);
			direction = Quaternion.Euler(new Vector3(0, 0, -angle * aMod)) * direction;
		}

		transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPos + direction * distance, 0.1f);
	}
}
