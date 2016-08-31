using UnityEngine;
using System.Collections;

public class SelectLevel : MonoBehaviour {


	public bool twoPlayer;
	public int WhichLevel = 0;
	public bool IsTimed = false;
	private GameObject numberOfPlayers;
	private float startTime;
	public int WaitTime;
	private GUIText text;

	void Start(){
		startTime = Time.time;
		numberOfPlayers = GameObject.Find ("NumberOfPlayers");
		DontDestroyOnLoad (numberOfPlayers);
		text =(GUIText)this.GetComponent(typeof(GUIText));
		GameObject Score = GameObject.Find ("Score");
		DontDestroyOnLoad (Score);
	}

	void Update(){
		if (IsTimed) {
			if(Time.time > startTime + WaitTime){
				Application.LoadLevel(WhichLevel);
			}
		}
	}
	void OnMouseEnter(){
		if (!IsTimed) {
			if(text!= null){
			text.color = Color.green;
			}
		}
}
	void OnMouseExit(){
		if (!IsTimed) {
			if(text != null){
			text.color = Color.white;
			}
		}
	}

	void OnMouseUp(){
		if (!IsTimed) {
			if(numberOfPlayers!=null){

				NumberOfPlayers numPlayerScript = (NumberOfPlayers)numberOfPlayers.GetComponent<NumberOfPlayers>();
				numPlayerScript.twoPlayer = twoPlayer;
			}

			Application.LoadLevel (WhichLevel); // if it starts over destroy score and numberofplayers objects
			if(WhichLevel == 0){
				Destroy(GameObject.Find("Score"));
				Destroy(numberOfPlayers);
			}
		}
	}
}