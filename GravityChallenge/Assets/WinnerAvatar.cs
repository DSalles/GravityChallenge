using UnityEngine;
using System.Collections;

public class WinnerAvatar : MonoBehaviour {
	public string ThisAvatar;
	// Use this for initialization
	void Start () {

		GameObject scoreKeeper = GameObject.Find ("Score");
		string winnerText = scoreKeeper.GetComponent<ScoreKeeping> ().WinnerText();
		if (ThisAvatar == "Astronaut1") {
			if (winnerText == "Player 1 wins!") {
				this.transform.position = new Vector3(0, transform.position.y,transform.position.z);
			} else if (winnerText == "Player 2 wins!") {
			this.gameObject.transform.position = new Vector3(0,12,0);
				//print ("WA destroyed " + ThisAvatar);
			}
		} else if (ThisAvatar == "Astronaut2") {
			if(winnerText == "Player 2 wins!"){
				this.transform.position = new Vector3(0, transform.position.y,transform.position.z);
			} else if(winnerText == "Player 1 wins!"){
				this.gameObject.transform.position = new Vector3(0,12,0);
			}
		}
	
	}
	

}
