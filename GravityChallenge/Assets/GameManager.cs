using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// Game Manager decides which hook is raised, which player gets points

public class GameManager : MonoBehaviour {
	public Material Astronaut1CarryingRock;
	public Material Astronaut2CarryingRock;
	public Texture2D[] RockTextures;
	public Material[] Astronaut1GroundRocks;
	public Material[] Astronaut2GroundRocks;
	public int RaiseHookedAstronautTime = 1;
	public int NumberOfRockDeliveries;
	private int NumberOfRocksPlayer1Delivered = 0;
	private int NumberOfRocksPlayer2Delivered = 0;
	public float AlertSpan;
	public string Planet;
	public int NextLoadWaitTime = 3;
	public static int Player1Score = 0;
	public static int Player2Score = 0;
	public static int Player3Score = 0;
	public GameObject Score;
	private ScoreKeeping scoreScript;
	public int NextLevel;
	private bool GameStarted = false;
	private bool LevelEnded = false;
	public int startPause = 3;
	public GUIText JumpText;
	public GUIText YouCanJumpXtimes;
	public HookLower Hook1;
	public HookLower Hook2;


	// Use this for initialization
	void Start () {

		// zero out total rocks delivered for this round
		NumberOfRocksPlayer1Delivered = 0;
		NumberOfRocksPlayer2Delivered = 0;
		GameStarted = false;
		LevelEnded = false;
		// make first carrying rock nothing
		Astronaut1CarryingRock.mainTexture = RockTextures[0];
		Astronaut2CarryingRock.mainTexture = RockTextures[0];
		// put this round's rocks on the ground
		for (int i = 0; i < Astronaut1GroundRocks.Length; i++) {
			Astronaut1GroundRocks [i].mainTexture = RockTextures [i+1];
		}
		for (int i = 0; i < Astronaut1GroundRocks.Length; i++) {
			Astronaut2GroundRocks [i].mainTexture = RockTextures [i+1];
		}
		// adjust gravity 
		if (Planet == "Moon") {
			Physics.gravity = new Vector3 (0, -1.622f, 0);
		} else if (Planet == "Mars") {
			Physics.gravity = new Vector3 (0, -3.711f, 0);
		} else if (Planet == "Venus") {
			Physics.gravity = new Vector3(0,-8.87f,0);
		}

	
		Score = GameObject.Find ("Score");

		if(Score != null) scoreScript = (ScoreKeeping)Score.GetComponent (typeof(ScoreKeeping));
	

	
	}

	public void AllPlayersCalibrated ()
	{
		// put a rock on the player
		Astronaut1CarryingRock.mainTexture = RockTextures [1];
		Astronaut2CarryingRock.mainTexture = RockTextures [1];
		// take it out of the ground
		Astronaut1GroundRocks[Mathf.Min (NumberOfRockDeliveries-1,NumberOfRocksPlayer1Delivered)].mainTexture = RockTextures[0];
		Astronaut2GroundRocks[Mathf.Min (NumberOfRockDeliveries-1,NumberOfRocksPlayer1Delivered)].mainTexture = RockTextures[0];
		if (!GameStarted) { // only start the game once
			if(JumpText!=null) {
				startRound sr = (startRound)JumpText.GetComponent(typeof(startRound));
				sr.activate(this);}
		}
	}

	public void GameStart(){
		YouCanJumpXtimes.transform.position = new Vector3(0, 0.95f,0);
		if(JumpText!= null) {StartCoroutine(JumpAlert());}
		if(Hook1!=null){
			Hook1.StartLowering (this, "Player 1", startPause );			
		}
		
		if(Hook2!=null){
			Hook2.StartLowering (this, "Player 2", startPause );
		}
		
		GameStarted = true;
	}

	public void Landed (string player)
	{
		if (player == "Player 1") {
			Astronaut1CarryingRock.mainTexture = RockTextures [Mathf.Min (RockTextures.Length - 1, NumberOfRocksPlayer1Delivered+1)];
			Astronaut1GroundRocks[Mathf.Min (NumberOfRockDeliveries-1,NumberOfRocksPlayer1Delivered)].mainTexture = RockTextures[0];
		} else if (player == "Player 2") {
			Astronaut2CarryingRock.mainTexture = RockTextures [Mathf.Min(RockTextures.Length-1, NumberOfRocksPlayer2Delivered+1)];
			Astronaut2GroundRocks[Mathf.Min (NumberOfRockDeliveries-1,NumberOfRocksPlayer1Delivered)].mainTexture = RockTextures[0];
		}
	}

	public void HookTriggered ( string hookName,  AstroJump ja, Transform hookTransform, Transform handTransform, Transform hookPointTransform  )
	{		
		if (LevelEnded)
		return;
		if (hookName == "Player 1") {
			StartCoroutine(Hook1.RaiseWithAstronaut(RaiseHookedAstronautTime, ja));
		}

		else if(hookName == "Player 2"){
			StartCoroutine(Hook2.RaiseWithAstronaut(RaiseHookedAstronautTime, ja));
		}

		// freeze the avatar that touched the hook
		ja.Hook (hookTransform, handTransform, hookPointTransform); 	

	}

	internal void AddToPlayerScore(string playerName, AstroJump aj)
	{
		if (LevelEnded)
			return;
		aj.Unhook ();
		if (playerName == "Player 1" && Astronaut1CarryingRock.mainTexture != RockTextures[0]) {
			NumberOfRocksPlayer1Delivered ++;
			if(scoreScript != null) scoreScript.AddToScore(playerName,Astronaut1CarryingRock.mainTexture);
			Astronaut1CarryingRock.mainTexture = RockTextures[0];
			// 
		}
		else if(playerName == "Player 2" && Astronaut2CarryingRock.mainTexture != RockTextures[0]) {
			NumberOfRocksPlayer2Delivered ++;
			scoreScript.AddToScore(playerName,Astronaut2CarryingRock.mainTexture);
			Astronaut2CarryingRock.mainTexture = RockTextures[0];// 
		}			
		checkIfLevelOver ();
	}

	void checkIfLevelOver(){
		if (LevelEnded)
			return;

			if(NumberOfRocksPlayer1Delivered == NumberOfRockDeliveries || NumberOfRocksPlayer2Delivered == NumberOfRockDeliveries)
		{
			LevelEnded = true;
			Hook1.StopAllCoroutines();
			Hook2.StopAllCoroutines();
			StartCoroutine(LoadNextLevel ());
		}
	}


	IEnumerator JumpAlert(){
		float startTime = Time.time;
		while (Time.time - startTime < AlertSpan) {
			JumpText.text = "Jump!";
			yield return null;
		}
		JumpText.text = "";
	}

	public IEnumerator LoadNextLevel(){
		float startTime = Time.time;
		while (Time.time - startTime < NextLoadWaitTime) {	
			JumpText.text = "Stop!";
			yield return null;
		}
		JumpText.text = "";
		Application.LoadLevel(NextLevel);
	}
}
