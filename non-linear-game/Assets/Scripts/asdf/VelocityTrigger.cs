using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTrigger : MonoBehaviour {

	public Rigidbody myRigidbody;

	public float movementSpeed;

	private float current;

	private float previous;

	private bool facingRight;

    [SerializeField]
    private Transform derp;


	// Use this for initialization
	void Start () {
		StartCoroutine (velFunction());
		facingRight = true;
		myRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame

	IEnumerator velFunction() {
		while (true) {
			previous = transform.localPosition.x;
			yield return new WaitForSeconds (0.1F);
			current = transform.localPosition.x;
			movementSpeed = ((current - previous) / Time.deltaTime) * 10;
			yield return null;
		}
	}

	void FixedUpdate () {
		Flip (movementSpeed);
	}

	void Flip (float movementSpeed) {
		if (movementSpeed > 0 && !facingRight || movementSpeed < 0 && facingRight) {
			facingRight = !facingRight;
		    var scale = this.derp.transform.localScale;

            this.derp.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);



		}
	}
}
