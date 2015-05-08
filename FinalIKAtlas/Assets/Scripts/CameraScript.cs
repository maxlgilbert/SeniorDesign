using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public GameObject viewDirection;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			Vector3 newPosition = viewDirection.transform.position;
			newPosition.y -= 2.5f;
			newPosition.x -= .5f;
			Camera.main.transform.position = newPosition;
		}
	
	}
}
