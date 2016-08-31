using UnityEngine;
using System.Collections;

public class WinnerText : MonoBehaviour {


	public TextMesh  Winner;
	public GameObject Score;
	ScoreKeeping scoreScript;
	// Use this for initialization
	void Start () {

		Score = GameObject.Find ("Score");
		scoreScript = (ScoreKeeping)Score.GetComponent (typeof(ScoreKeeping));		
	}
	
	// Update is called once per frame
	void Update (){ 
			Winner.text = scoreScript.WinnerText ();

	}
}
