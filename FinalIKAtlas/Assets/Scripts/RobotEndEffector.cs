using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotEndEffector : MonoBehaviour {
	public Vector3 velocity;
	private List<Vector3> _targets;
	public float speed = 10.0f;
	public float frameConstant = 300.0f;
	private Vector3 effectorVelocity;
	//private Vector3 _currentTarget;
	private int _currentTarget = -1;
	public float epsilon = .01f;
	public string moCapName = "";
	// Use this for initialization
	void Awake () {
		//_currentTarget = new Vector3();
		_targets = new List<Vector3>();
	}
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (Input.GetKeyDown(KeyCode.P)&&_currentTarget<0) {
			if (!string.IsNullOrEmpty(moCapName)){
				this._targets = MoCapReader.instance.effectorPositions[moCapName];
				UpdateTarget();
			}
		}
		
		if (_targets.Count>_currentTarget){
			if (_currentTarget >= 0){
				if (Vector3.Distance(gameObject.transform.position,_targets[_currentTarget])>epsilon){
					this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x+effectorVelocity.x,
					                                                 this.gameObject.transform.position.y+effectorVelocity.y,
					                                                 this.gameObject.transform.position.z+effectorVelocity.z);
				} else {
					UpdateTarget();
				}
			}
		}
	}

	public void UpdateTarget(){
		
		if (_targets.Count>_currentTarget+1){
			Debug.LogError("Switched");
			_currentTarget++;
			effectorVelocity = (_targets[_currentTarget]-gameObject.transform.position)/(speed*frameConstant);
		}
	}
}
