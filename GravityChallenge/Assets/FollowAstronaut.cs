using UnityEngine;
using System.Collections;

public class FollowAstronaut : MonoBehaviour {
	public Transform AstronautRoot;
	// Use this for initialization
	void Update () {
		this.transform.position = AstronautRoot.position;
	}
	

}
