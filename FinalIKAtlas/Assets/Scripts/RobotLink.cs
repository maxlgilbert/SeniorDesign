using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

public class RobotLink : RobotPart {
	public GameObject mesh;
	public List<RobotLink> children;
	public RobotLink parent;
	public RobotJoint parentJoint;
	public RotationLimitHinge rotationLimit;
	public Matrix4x4 rotationTransformation;
	//public GameObject me
	//public bool skip = false;

	// Use this for initialization
	void Awake () {
		//mesh = gameObject.GetComponentInChildren<RobotMesh>();
		children = new List<RobotLink>();
		rotationTransformation = Matrix4x4.identity;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
