using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransitions : MonoBehaviour {

	private Animator myAnimator;

	public float parentMovement;

	// Use this for initialization
	void Start () {
		myAnimator = GetComponent<Animator>();
	}

	void triggerWalk() {
		parentMovement = GetComponentInParent<VelocityTrigger> ().movementSpeed;
		if (parentMovement != 0) {
			myAnimator.SetBool ("isMoving", true);
		} else {
			myAnimator.SetBool ("isMoving", false);
		}
	}

	// Update is called once per frame
	void Update () {
		triggerWalk ();
	}
}
