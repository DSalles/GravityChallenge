
using UnityEngine;
using System.Collections;

public class FloatingInSpace : MonoBehaviour {
//	public float Speed;
	public Transform RotationAxisSource;
	public string player;
	private Vector3 RotationAxis;

	public KinectManager kinectManager;
	// Use this for initialization
	void Start () {

	}

	void Update () {
		Vector3 dirJoints = Vector3.zero;
		RotationAxis = new Vector3 (RotationAxisSource.position.x, RotationAxisSource.position.y, RotationAxisSource.position.z);
		if (player == "Player1") {
			 dirJoints = kinectManager.GetDirectionBetweenJoints (kinectManager.GetPlayer1ID (), 1, 2, false, false);
		}else if(player =="Player2"){
			 dirJoints = kinectManager.GetDirectionBetweenJoints (kinectManager.GetPlayer2ID (), 1, 2, false, false);
		}
		float roll = dirJoints.x * 100;
		transform.RotateAround (RotationAxis, Vector3.left, roll * Time.deltaTime / 2);
		float pitch = dirJoints.z * -100;
		transform.RotateAround (RotationAxis, Vector3.forward, pitch * Time.deltaTime / 2);
		transform.position = new Vector3 (transform.position.x + roll*.001f, transform.position.y, transform.position.z - pitch*.001f);
		//print ("dirJoints " + dirJoints);
			//= Quaternion.Lerp (transform.rotation, newRotation, Time.deltaTime/2);
	}
}
