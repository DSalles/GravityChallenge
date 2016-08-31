using UnityEngine;
using System.Collections;

public class AstroJump : MonoBehaviour {
	public float speed = 45;

	float ForceMultiplier = 150; // to make it so "Speed" is aout equal to the gravitational difference e.g. 6 times higher on the moon, etc. 
	public GameManager GM;
	public string Player;
	//public GameObject Core;
	public bool Grounded = false;
	//public float previousCoreY;
	AvatarControllerClassic AvatarScript;

	//Vector3 BodyRoot;

	public float BodyRootY;

	public float OldBodyRootPosY;

	// Use this for initialization
	void Start () {
		GM = (GameManager)GameObject.Find("GameManager").GetComponent<GameManager>();
		AvatarScript = (AvatarControllerClassic)this.GetComponentInParent(typeof(AvatarControllerClassic));
		//BodyRoot = AvatarScript.GetHipPos(); // used to get real height of player hips for jumping
		if(AvatarScript) AvatarScript.RegisterAstroJump (this);
		BodyRootY = 0;
		OldBodyRootPosY = 0;
		//previousCoreY = Core.transform.localPosition.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Hooked || !AvatarScript || !AvatarScript.GetPlayerDetected()) {
			OldBodyRootPosY = 0; // this should fix the jumping casued by player loss
			return;
		}
	
		BodyRootY = AvatarScript.GetHipPos().y;

		if (Grounded && OldBodyRootPosY !=0) {

			float deltaCoreY = (BodyRootY - OldBodyRootPosY);

			if (deltaCoreY > 0.01f) {
				//print ("DeltaCoreY " + deltaCoreY);
				GetComponent<Rigidbody>().AddForce (new Vector3 (0, Mathf.Max(0,Mathf.Min(150,deltaCoreY * ForceMultiplier* speed)), 0) );
			//	print ("AJ deltaCore Y * Fm * speed " + deltaCoreY * ForceMultiplier * speed);
			}
		}
		OldBodyRootPosY = BodyRootY;	
	}


	// make astronaut grounded thus able to jump only when touching ground
	void OnTriggerEnter(Collider other){
	//	print ("Triggered by : " + other.name);
		if (other.tag == "Ground") {
			GM.Landed(Player);
			Grounded = true;
		}
	}

	void OnTriggerExit (Collider other){
		if (other.tag == "Ground") {
			Grounded = false;
		}
	}

	bool Hooked = false;

	Transform HookTransform;



	public void Hook(Transform hookTransform, Transform HookPoint, Transform Hand){
		//this is called from the game manager when the hook detects a hand
		if (Hooked) {
			return;
		}
		Hooked = true;
	
		HookTransform = hookTransform;

		AvatarScript.Hook (); // freeze all skeletal movement
		this.transform.parent = hookTransform;
		stickyHands = StickHandToHook (Hand, HookPoint);
		StartCoroutine(stickyHands);
	}
	IEnumerator stickyHands;
	IEnumerator StickHandToHook(Transform hand, Transform hookPoint){
		while (1==1) {
			float xDiff = hand.transform.position.x - hookPoint.transform.position.x;
			float yDiff = hand.transform.position.y - hookPoint.transform.position.y;
			float zDiff = hand.transform.position.z - hookPoint.transform.position.z;
			this.transform.position =  Vector3.Lerp(this.transform.position,new Vector3(this.HookTransform.position.x + xDiff, this.HookTransform.position.y + yDiff, this.HookTransform.position.z + zDiff),Time.deltaTime);
			//print("JA stickHandToHook");
			yield return null;}
	}
	public void Unhook(){ // called from game manager when hook finishes its raise coroutine
		if (!Hooked)
			return;
		Hooked = false;

		AvatarScript.Unhook ();

		this.transform.parent = null;
		StopCoroutine (stickyHands);
	}

}
