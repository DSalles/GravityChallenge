using UnityEngine;
using System.Collections;

public class startRound : MonoBehaviour {

	bool Activated = false;

	private GameManager GM;
    public bool OnlyStartOnClick = false;

	public void activate(GameManager gm){
        GM = gm;
		Activated = true;
        if (OnlyStartOnClick) this.GetComponent<GUIText>().text = "Start";
        else gm.GameStart();
		
	}
	void OnMouseUp(){

        if (Activated && OnlyStartOnClick)
        {
			GM.GameStart();
		}
	}
}
