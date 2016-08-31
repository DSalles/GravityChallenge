using UnityEngine;
using System.Collections;
  
public class HookDetector : MonoBehaviour {
	// the hand hook triggers
	// this can tell another collider what body is connected to this
	public GameObject BodyRoot;
	public Rigidbody rigidBody;
	public Transform ThisHand;
	private AstroJump astroJump;

	public AstroJump GetAstroJump(){
		return astroJump;
	}

	public Rigidbody GetRigidBody(){
		return rigidBody;
	}

	public Transform GetThisHand(){
		return ThisHand;
	}

	void Start(){
		ThisHand = (Transform)this.gameObject.transform; 
		astroJump = (AstroJump)BodyRoot.GetComponent(typeof(AstroJump));
		rigidBody = (Rigidbody)BodyRoot.GetComponent (typeof(Rigidbody));
	}

/*	void OnTriggerEnter(Collider other) {
		if (other.tag == "Hook") {
			rigidBody.useGravity = false;
			jumpAmplifier.Hook ();
		}
	}
	void OnTriggerStay(Collider other) {
		if (other.tag == "Hook") {
			rigidBody.useGravity = false;
		}

	}*/
}
