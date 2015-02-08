using UnityEngine;
using System.Collections;

public class RobotJoint : RobotPart {

	public RobotLink parent;
	public RobotLink child;
	public float limitMin;
	public float limitMax;
	public Matrix4x4 rotationTransformation;
	//public bool skip;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
