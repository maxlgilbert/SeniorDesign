using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

public class RobotEndEffector : MonoBehaviour {
	public Vector3 velocity;
	public List<Vector3> _targets;
	private float speed = 1.0f;
	public float frameConstant = 60.0f;
	private Vector3 effectorVelocity;
	//private Vector3 _currentTarget;
	private int _currentTarget = -1;
	public float epsilon = .01f;
	public string moCapName = "";
	public GameObject endEffectorObject;
	public FABRIKRoot fabrikRoot;
	private FABRIK fabrik;
	public int chainNumber;
	private int _currentFrame = 0;
	private int _timeScale = 1;
	private GameObject _endEffectorObject;
	// Use this for initialization
	void Awake () {
		//_currentTarget = new Vector3();
		_targets = new List<Vector3>();
	}
	void Start () {
		_endEffectorObject = GameObject.Instantiate (endEffectorObject, gameObject.transform.position, gameObject.transform.localRotation) as GameObject;
		//IKSolverFABRIKRoot solver = fabrikRoot.GetIKSolver() as IKSolverFABRIKRoot;
		fabrik = gameObject.GetComponentInParent<FABRIK> () as FABRIK;
		//IKSolverFABRIK solver = fabrikRoot.GetIKSolver() as IKSolverFABRIKRoot;
		//solver.chains [chainNumber].ik.solver.IKPosition = (new Vector3(1, 0.0f, 0.0f));
		//IKSolver.Point[] points = solver.GetPoints ();
		//points [chainNumber] = endEffector.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown(KeyCode.P)&&_currentTarget<0) {
			if (!string.IsNullOrEmpty(moCapName)){
				this._targets = MoCapReader.instance.effectorPositions[moCapName];
				UpdateTarget();
			}
		}

		_currentFrame++;
		if (_targets.Count>_currentTarget){
			if (_currentTarget >= 0 && _currentFrame%_timeScale == 0){
				//if (Vector3.Distance(gameObject.transform.position,_targets[_currentTarget])>epsilon){
				//} else {
					UpdateTarget();
				//}
			}
		}
	}

	public void UpdateTarget(){
		
		if (_targets.Count > _currentTarget + 1) {
			//Debug.LogError("Switched");
			_currentTarget++;
			fabrik.solver.IKPosition = _targets [_currentTarget];
			_endEffectorObject.transform.position = _targets [_currentTarget];
			//Debug.LogError(_targets[_currentTarget]);
			//effectorVelocity = (_targets[_currentTarget]-fabrik.solver.IKPosition )/(speed*frameConstant);
			//StartCoroutine("SwitchTarget");
		} else {
			Debug.LogError("Done!");
		}
	}

	public IEnumerable SwitchTarget(){
		yield return new WaitForSeconds(.25f);
		UpdateTarget ();
	}
}
