
using UnityEngine;
using System.Collections;

public class JumpAmplifier : MonoBehaviour {

	public GameManager GM;
	public string Player;
	public GameObject Avatar;
	private Vector3 BodyRoot;
	//private Vector3 HookPos;
	public float OldBodyRootPosY;
	internal float BodyRootY;
	public float Amplifier = 2;
	public bool ReadyToJump = true;
	public float JumpStartTime = 0;
	private float JumpEndTime = 0;
	public float JumpTime = 0;
	public float MaxFloatSeconds = 3;
	public bool AdditionalHeight = false; // when avatar jumps higher after player jump stops 
	public float GroundHeight = 0;
	public float JumpHeight;
	private int MAXHEIGHT = 5;
	public float AdditionalHeightFactor = 4;
	public Rigidbody RgdBody;
	public float JumpTolerance = 0.04f;
	public float HeightTolerance = 0.15f;
	public bool Jumping = false;
	public float LandTime = 0;
	public float JumpPause = 0.1f;
	public float JumpTimeMultiplier = 12;
	private Transform HookTransform;
	AvatarControllerClassic AvatarScript;
	public bool Hooked = false;

	// Use this for initialization
	void Start () {
		//this.transform.position = new Vector3 (this.transform.position.x, 0, this.transform.position.z);
		AvatarScript = (AvatarControllerClassic)Avatar.GetComponent(typeof(AvatarControllerClassic));
		RgdBody = (Rigidbody)GetComponent (typeof(Rigidbody));
		BodyRoot = AvatarScript.GetHipPos(); // used to get real height of player hips for jumping
	//	if(AvatarScript) AvatarScript.RegisterJumpAmplifier (this);
		BodyRootY = BodyRoot.y;
		OldBodyRootPosY = BodyRootY;
		RgdBody.useGravity = false;
		GroundHeight = this.transform.position.y;
		JumpHeight = GroundHeight;

	}
	public void Hook(Transform hookTransform, Transform HookPoint, Transform Hand){
		//this is called from the game manager when the hook detects a hand
		if (Hooked) {
			return;
		}
		Hooked = true;
		RgdBody.useGravity = false;
		RgdBody.mass = 0.01f;
		HookTransform = hookTransform;
		if (elevateCoroutine!= null) {
			StopCoroutine (elevateCoroutine);
		}
		AvatarScript.Hook (); // freeze all skeletal movement
		this.transform.parent = hookTransform;
		stickyHands = StickHandToHook (Hand, HookPoint);
		StartCoroutine(stickyHands);
	}

	public void Unhook(){ // called from game manager when hook finishes its raise coroutine
		if (!Hooked)
			return;
		Hooked = false;
		RgdBody.useGravity = true;
		AvatarScript.Unhook ();
		RgdBody.mass = 1;
		this.transform.parent = null;
		StopCoroutine (stickyHands);
	}

	void OnTriggerStay(Collider other){ //Astronaut waits to jump again
		if (other.tag == "Ground") {
			if(!ReadyToJump && !Jumping && Time.time > LandTime + JumpPause ){
				ReadyToJump = true;
			}
		}
	}

	void OnTriggerEnter(Collider  other) {
		if (other.tag == "Ground" ) {
	//		if(Jumping){ // tell game manager player has landed
			GM.Landed(Player);//}
			LandTime = Time.time; // start wait to jump
			RgdBody.useGravity = false; // keep it from bouncing
	//		Jumping = false;
		} 
	}
	void OnTriggerExit(Collider other){
		if (other.tag == "Ground") {
			//Jumping = true;
			JumpStartTime = Time.time;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Hooked) {
				return;
		}

		if (!AvatarScript || !AvatarScript.GetPlayerDetected()) {
			OldBodyRootPosY = 0; // this should fix the jumping casued by player loss
			return;
		}

		BodyRootY = AvatarScript.GetHipPos().y;

		if (OldBodyRootPosY != 0) {
			if (ReadyToJump && ((BodyRootY > OldBodyRootPosY + JumpTolerance))) {
				Jumping = true;
				ReadyToJump = false;
				StartCoroutine (AmplifyPlayerHeight ());
			} //else if (Jumping) { // player started going down
			//	RgdBody.useGravity = true;
			//}
		}

		OldBodyRootPosY = BodyRootY;	
	}

	IEnumerator elevateCoroutine;

	IEnumerator AmplifyPlayerHeight(){
		float AmplifierAmplifier = 1;

		while ((BodyRootY > OldBodyRootPosY + JumpTolerance)) {
			//print ("Amplify");
			//move this up
			float elevation = Mathf.Max(0,Mathf.Min(this.transform.position.y + ((BodyRootY - OldBodyRootPosY) * Amplifier* AmplifierAmplifier),MAXHEIGHT));
			this.transform.position = new Vector3 (this.transform.position.x, elevation, this.transform.position.z);
			AmplifierAmplifier *=1.2f;

			yield return null;
		} 

			JumpEndTime = Time.time;
			JumpTime = JumpEndTime - JumpStartTime;
			JumpHeight = (this.transform.position.y - GroundHeight) ;
			elevateCoroutine = AddHeight(this.transform.position, new Vector3(this.transform.position.x, JumpHeight * AdditionalHeightFactor, this.transform.position.z),Mathf.Min (MaxFloatSeconds,JumpTime*JumpTimeMultiplier));
		Jumping = false;
		StartCoroutine(elevateCoroutine);	

	}

	IEnumerator AddHeight (Vector3 startpos, Vector3 endpos, float seconds ) {

		float elapsedTime = 0.0f;

		while (elapsedTime <= seconds) {
			elapsedTime += Time.deltaTime;
			//float smoothout = (elapsedTime*(Mathf.Lerp(1.2f,1,elapsedTime/seconds))/seconds);
			this.transform.position = Vector3.Lerp(startpos, endpos,elapsedTime/seconds);// smoothout);
			yield return null;
		}
		RgdBody.useGravity = true;
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
}
