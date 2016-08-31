using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class HeadRenderer : MonoBehaviour {

	public RawImage rawImage;
	public KinectWrapper.NuiSkeletonPositionIndex TrackedHead = KinectWrapper.NuiSkeletonPositionIndex.Head;
	public float smoothFactor = 5f;
	public float HeadImageHeight = 0.07f;
	public float HeadImageWidth = 0.08f;
	public Texture2D DefaultAstronautHead;



	// Update is called once per frame
	void Update () {
		KinectManager manager = KinectManager.Instance;
		if (manager && manager.IsInitialized ()) {
			int HeadIndex = (int)TrackedHead;


			if (manager.IsUserDetected ()) {
				if (rawImage) {
					rawImage.texture = manager.GetUsersClrTex();
				}
				long userId = manager.GetPlayer1ID ();
			
				if (manager.IsJointTracked (userId, HeadIndex)) {
					Vector3 posJoint = manager.GetRawSkeletonJointPos (userId, HeadIndex);
				
					if (posJoint != Vector3.zero) {
					//	print ("posJoint");
						// 3d position to depth
						Vector2 posDepth = manager.GetDepthMapPosForJointPos (posJoint);
					
						// depth pos to color pos
						Vector2 posColor = manager.GetColorMapPosForDepthPos (posDepth);
					
						float scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
						float scaleY = (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;
						scaleX -= HeadImageWidth / 2.5f;
						scaleY -= HeadImageHeight / 5f;
					
						//						Vector3 localPos = new Vector3(scaleX * 10f - 5f, 0f, scaleY * 10f - 5f); // 5f is 1/2 of 10f - size of the plane
						//						Vector3 vPosOverlay = backgroundImage.transform.TransformPoint(localPos);
						//Vector3 vPosOverlay = BottomLeft + ((vRight * scaleX) + (vUp * scaleY));
					
					
					
						if (rawImage) {
						//	print ("rawInage");
							rawImage.uvRect = new Rect (Mathf.Lerp (rawImage.uvRect.x, scaleX, smoothFactor * Time.deltaTime), Mathf.Lerp (rawImage.uvRect.y, scaleY, smoothFactor * Time.deltaTime), HeadImageHeight, HeadImageWidth);
							//	Vector3 vPosOverlay = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
							//	OverlayObject.transform.position = Vector3.Lerp(OverlayObject.transform.position, vPosOverlay, smoothFactor * Time.deltaTime);
						}
					}
				}
			
			}
		}
	}
}
