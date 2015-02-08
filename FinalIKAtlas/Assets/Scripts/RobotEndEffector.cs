using UnityEngine;
using System.Collections;

public class RobotEndEffector : MonoBehaviour {
	public Vector3 velocity;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x+velocity.x,
		                                                 this.gameObject.transform.position.y+velocity.y,
		                                                 this.gameObject.transform.position.z+velocity.z);
	}
}
