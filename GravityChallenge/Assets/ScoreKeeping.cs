using UnityEngine;
using System.Collections;

public class ScoreKeeping : MonoBehaviour {
	public Texture nothing;
	public GUIText	player1ScoreText;
	public GUIText player2ScoreText;
	public Material[] Player1RockCollection;
	public Material[] Player2RockCollection;

	private int player1ScoreNumber;
	private int player2ScoreNumber;

	void start(){
		player1ScoreNumber=0;
		player2ScoreNumber=0;
		if(player1ScoreText != null)
		player1ScoreText.text = "";
		if(player2ScoreText != null)
		player2ScoreText.text = "";

	}

	public void AddToScore(string player, Texture rockTexture){
		if (player == "Player 1") {
			Player1RockCollection[player1ScoreNumber].mainTexture = rockTexture;
			player1ScoreNumber ++;
			if(player1ScoreText !=null)
			player1ScoreText.text = "Player 1 collected: " + player1ScoreNumber.ToString();
		} else if (player == "Player 2") {
			Player2RockCollection[player2ScoreNumber].mainTexture = rockTexture;
			player2ScoreNumber ++;
			if(player2ScoreText != null)
			player2ScoreText.text = "Player 2 collected: " + player2ScoreNumber.ToString();
		}
	}

	public int PlayerScore(string playerName){
		int i = 0;
		if (playerName == "Player 1") {
			i = player1ScoreNumber;
		} else if (playerName == "Player 2") {
			i = player2ScoreNumber;
		}
		return i;

	}

	public string WinnerText(){
		// reset all rock textures to nothing
		for (int i = 0; i < Player1RockCollection.Length; i++) {
			
			Player1RockCollection[i].mainTexture = nothing;
		}
		for (int i = 0; i < Player2RockCollection.Length; i++) {
			
			Player2RockCollection[i].mainTexture = nothing;
		}
		string winningPlayer = "no winner";
		if (player1ScoreNumber > 0 && player1ScoreNumber == player2ScoreNumber) //tie
			winningPlayer = "It's a Tie!";
		else if (player1ScoreNumber > player2ScoreNumber)
			winningPlayer = "Player 1 wins!";
		else if (player1ScoreNumber < player2ScoreNumber)
			winningPlayer = "Player 2 wins!";

		return winningPlayer;
	}
}
