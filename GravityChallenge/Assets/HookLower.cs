using UnityEngine;
using System.Collections;

public class HookLower : MonoBehaviour {

	public Vector3 EndPos;
	public float SecondsToLowerAndReturn = 5;
	public float HookRepeatSlowDownFactor;
	public Vector3 LeavePos;
	public Transform HookPoint;
	private Vector3 StartPos;
	private int startPause = 3;
	private bool Started = false;
	private bool Tripped = false;
	private GameManager GM;
	private string HookName;


	public void StartLowering(GameManager gm, string name,  int startpause){
		GM = gm;
		Started = false;
		Tripped = false;
		StartPos = transform.position;
		startPause = startpause;
		HookName = name;
		StartCoroutine (Wait());
	}

	void OnTriggerEnter (Collider other){
		//print ("Trigger!");
		if (!Started || Tripped) {
			return;
		}
		if (other.tag == "Hand") {
			Tripped = true;
			StopCoroutine ("Lower");
			StopCoroutine("Return");
			HookDetector hd = (HookDetector)other.GetComponent(typeof(HookDetector));	
			AstroJump ja = hd.GetAstroJump();
			Transform hand = hd.GetThisHand();
			//ja.Hook(this.transform); this is now done in gameManager
			GM.HookTriggered (HookName, ja, this.transform, hand, HookPoint);		
		}
	}

	public IEnumerator Wait(){
		float startTime = Time.time;
		while (Time.time - startTime < startPause) {
		yield return null;
		}
		StartCoroutine ("Lower");
		Tripped = false; // reset the trigger
		Started = true;
	}

	public IEnumerator Lower()
	{
		var currentPos = transform.position;
		var t = 0f;
		while(t < 1)
		{
			t += Time.deltaTime / SecondsToLowerAndReturn;
			transform.position = Vector3.Lerp(currentPos, EndPos, t);
			yield return null;
		}
		StartCoroutine ("Return");
		//GM.HookLanded(this, HookName);
	}

	public IEnumerator Return()
	{
		var currentPos = transform.position;
		var t = 0f;
		while(t < 1)
		{
			t += Time.deltaTime / SecondsToLowerAndReturn;
			transform.position = Vector3.Lerp(currentPos, StartPos, t);
			yield return null;
		}
		StartCoroutine ("Lower");
		//GM.HookLanded(this, HookName);
	}

	public float TimeToRaiseWithAstronaut = 1f;

	// why is this called from outside instead of from OnTriggerEnter 
	public IEnumerator RaiseWithAstronaut(float time, AstroJump aj)
	{ //Lift Astronaut
	
		var currentPos = transform.position;
		var t = 0f;
		while(t < TimeToRaiseWithAstronaut)
		{
			t += Time.deltaTime / time;
			transform.position = Vector3.Lerp(currentPos, LeavePos, t);
			yield return null;
		}	

		GM.AddToPlayerScore (HookName, aj);

		SecondsToLowerAndReturn *= HookRepeatSlowDownFactor;

		StartCoroutine (Wait());

	}
}
