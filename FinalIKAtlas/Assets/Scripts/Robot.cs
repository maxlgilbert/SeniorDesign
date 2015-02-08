using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
//using RootMotion.FinalIK;

public class Chain {
	public RobotLink rootLink;
	public Chain parentChain;
	public List<Chain>  childrenChains;
	public int index;
	public Chain () {
		childrenChains = new List<Chain>();
	}

}

public class Robot : MonoBehaviour {
	private RobotLink _root;
	private FABRIKRoot _FABRIKRoot;
	private IKSolverFABRIKRoot _FABRIKRootSolver;
	private List<Chain> _chains;
	private Dictionary<int,Chain> _chainDictionary;
	private int _numChains;
	private int _currChainIndex;
	private List<FABRIKChain> _FABRIKChain;
	private List<RotationLimit> _rotationLimits;
	private List <FABRIK> _fabriks;
	private Dictionary<string,RobotLink> _robotLinks;
	public string outputLocation;
	private int _frame = 1;
	private string _robotInfo = "";
	private List<RobotLink> _skippedLinks;

	public GameObject meshObject;

	// Use this for initialization
	void Awake () {
		_chains = new List<Chain>();
		_numChains = 0;
		_currChainIndex = 0;
		_chainDictionary = new Dictionary<int, Chain>();
		_rotationLimits = new List<RotationLimit>();
		_fabriks = new List<FABRIK>();
		_skippedLinks = new List<RobotLink>();
	}
	
	// Update is called once per frame
	void Update () {
		/*if (!_rotationLimits[0].enabled) {
			for (int i = 0; i < _rotationLimits.Count; i++) {
				//_rotationLimits[i].enabled=true;
			}
		}
		if (!_fabriks[0].enabled) {
			for (int i = 0; i < _fabriks.Count; i++) {
				//_fabriks[i].enabled=true;
			}
		}*/
		if (_robotLinks != null){
			if(Input.GetKeyDown(KeyCode.P)) {
				_robotInfo += _frame + "\n";
				foreach(string key in _robotLinks.Keys) {
					RobotLink link = _robotLinks[key];
					_robotInfo += key + " ";
					_robotInfo += link.gameObject.transform.position + " ";
					_robotInfo += link.gameObject.transform.rotation + "\n";
				}
				System.IO.File.WriteAllText(outputLocation,_robotInfo);
				_frame++;
			}
		}

	
	}

	public void BuildRobot (Dictionary<string,RobotLink> RobotLinks,Dictionary<string,RobotJoint> RobotJoints){
		_robotLinks = RobotLinks;
		List<RobotJoint> RobotJointValues = new List<RobotJoint>();
		foreach(RobotJoint joint in RobotJoints.Values) {
			RobotJointValues.Add(joint);
		}
		List <RobotLink> SkippedLinks = new List<RobotLink>();
		for (int i = 0; i < RobotJointValues.Count; i++) {
			RobotJoint joint = RobotJointValues[i];
			RobotLink parent = joint.parent;
			RobotLink child = joint.child;
			if (child != null) {
				child.parentJoint = joint;
			}
			//RotationLimitHinge rLH = null;
			bool skipped = false;
			if (joint.originXYZ == new Vector3()) {
				skipped = true;
				SkippedLinks.Add(child);
				_skippedLinks.Add(child);
				//Debug.LogError(child.name);
			}
			/*if (parent.name.Equals("pelvis")) {
				rLH = parent.gameObject.AddComponent<RotationLimitHinge>() as RotationLimitHinge; //TODO parent or child?
				rLH.min = 0.0f;
				rLH.max = 0.0f;
				rLH.axis = new Vector3(0.0f,0.0f,1.0f);
				_rotationLimits.Add(rLH);
				parent.rotationLimit = rLH;
				//parent.gameObject.transform.rotation = Quaternion.Euler(-90.0f,-180.0f,0.0f);
			}*/
			if (child!=null) 
			{
				if (!skipped) {
//					rLH = child.gameObject.AddComponent<RotationLimitHinge>() as RotationLimitHinge; //TODO parent or child?
//					rLH.min = joint.limitMin*180.0f/Mathf.PI;
//					rLH.max = joint.limitMax*180.0f/Mathf.PI;
//					rLH.axis = joint.axisXYZ;
//					_rotationLimits.Add(rLH);
//					child.rotationLimit = rLH;
				}
			}

			parent.children.Add(child);
			if (child != null)
			{
				child.parent = parent;
			}
			//parent.mesh.gameObject.transform.localScale = new Vector3(joint.originXYZ.x+1,joint.originXYZ.y+1,1);
			//parent.mesh.gameObject.transform.localPosition = new Vector3(1,joint.originXYZ.y/2.0f,1);


			/*child.gameObject.transform.position = joint.originXYZ;
			child.gameObject.transform.rotation =  Quaternion.Euler(joint.originRPY);
			child.gameObject.transform.parent = parent.gameObject.transform;*/
			//joint.originXYZ.z-=.06f;
		}
		foreach(RobotLink link in RobotLinks.Values) {
			if(link.parent == null) {
				_root = link;
			}
		}

		// Create mesh transforms for robot
		RobotLink parentLink = _root;
		CreateMesh(parentLink);
		//_root.gameObject.transform.position = new Vector3();
		//_root.gameObject.transform.rotation = Quaternion.Euler(-90.0f,-180.0f,0.0f);


		
		for (int i =0; i < SkippedLinks.Count; i++) {
			RobotLink curr = SkippedLinks[i];
			RobotLink parent = curr.parent;
			RobotJoint parentJoint = curr.parentJoint;
			curr.gameObject.transform.parent = parent.gameObject.transform.parent;
			for (int j = 0; j < curr.children.Count; j++) {
				curr.children[j].parent = parent;
				curr.children[j].parentJoint = parentJoint;
				curr.children[j].gameObject.transform.parent = parent.gameObject.transform;
			}
			parent.children.Remove(curr);
			parent.children.AddRange(curr.children);
		}

		GameObject gameObjectFABRIKRoot = new GameObject();
		gameObjectFABRIKRoot.transform.parent = gameObject.transform;
		_FABRIKRoot = gameObjectFABRIKRoot.AddComponent<FABRIKRoot>() as FABRIKRoot;
		_FABRIKRootSolver = _FABRIKRoot.GetIKSolver() as IKSolverFABRIKRoot;
		Chain rootChain = new Chain();
		Chain kinematicChain =  CreateKinematicChain(_root, rootChain);
		_FABRIKChain = new List<FABRIKChain>();
		LinkMultipleKinematicChains(kinematicChain);
		_FABRIKRootSolver.chains = new FABRIKChain[_FABRIKChain.Count];
		for (int i = 0; i < _FABRIKChain.Count; i++) {
			Chain currChain = _chainDictionary[i];
			int []indexArray = new int[currChain.childrenChains.Count];
			for (int j = 0; j < currChain.childrenChains.Count; j++) {
				indexArray[j] = currChain.childrenChains[j].index;
			}
			_FABRIKChain[i].children = indexArray;
			_FABRIKRootSolver.chains[i] = _FABRIKChain[i];
		}
	}

	public void CreateMesh(RobotLink parentLink) {
		foreach (RobotLink childLink in parentLink.children) {
			if (childLink != null)
			{
				childLink.gameObject.transform.parent = parentLink.gameObject.transform;
				childLink.gameObject.transform.localRotation = Quaternion.Euler(childLink.parentJoint.originRPY);
				childLink.gameObject.transform.localPosition = childLink.parentJoint.originXYZ;
				RotationLimitHinge rLH = parentLink.gameObject.AddComponent<RotationLimitHinge>() as RotationLimitHinge; //TODO parent or child?
				rLH.min = childLink.parentJoint.limitMin*180.0f/Mathf.PI;
				rLH.max = childLink.parentJoint.limitMax*180.0f/Mathf.PI;

				// Set axis of rotation
				/*Vector4 homogenousAngle = new Vector4(childLink.parentJoint.axisXYZ.x,
				                                      childLink.parentJoint.axisXYZ.y,
				                                      childLink.parentJoint.axisXYZ.z,
				                                      0);
//				float alpha = parentLink.GetComponent<RotationLimitHinge>().axis.x;
//				float beta = parentLink.GetComponent<RotationLimitHinge>().axis.y;
//				float gamma = parentLink.GetComponent<RotationLimitHinge>().axis.z;
				float alpha = childLink.parentJoint.axisXYZ.x;
				float beta = childLink.parentJoint.axisXYZ.y;
				float gamma = childLink.parentJoint.axisXYZ.z;

				foreach(RobotLink skipped in _skippedLinks) {
					if (skipped.parentJoint.parent.Equals(parentLink)) {
						Vector4 homogenousAngleSkipped = new Vector4(skipped.parentJoint.axisXYZ.x,
						                                             skipped.parentJoint.axisXYZ.y,
						                                             skipped.parentJoint.axisXYZ.z,
						                                             0);
						Matrix4x4 transformMatSkipped = GetRotationMatrix(homogenousAngleSkipped.x,homogenousAngleSkipped.y,homogenousAngleSkipped.z);
						Vector4 newAngles = transformMatSkipped * homogenousAngleSkipped;
						alpha = newAngles.x;
						beta = newAngles.y;
						gamma = newAngles.z;
					}
				}

				
				Matrix4x4 transformMat = GetRotationMatrix(alpha,beta,gamma);
				childLink.rotationTransformation = parentLink.rotationTransformation*transformMat.inverse;
				homogenousAngle= parentLink.rotationTransformation*homogenousAngle;
				rLH.axis.x = homogenousAngle.x;
				rLH.axis.y = homogenousAngle.y;
				rLH.axis.z = homogenousAngle.z;*/
				//childLink.parentJoint.axisXYZ = rLH.axis;
				//rLH.axis = childLink.parentJoint.axisXYZ;
				rLH.axis.x = childLink.parentJoint.axisXYZ.x;
				rLH.axis.y = childLink.parentJoint.axisXYZ.y;
				rLH.axis.z = childLink.parentJoint.axisXYZ.z;
				//_rotationLimits.Add(rLH);
				childLink.rotationLimit = rLH;
				CreateMesh(childLink);
			}
		}
	}

	public Matrix4x4 GetRotationMatrix (float alpha, float beta, float gamma) {
		Matrix4x4 transformMat = new Matrix4x4();
		Matrix4x4 xAxis = new Matrix4x4();
		xAxis [0,0] = 1;
		xAxis [0,1] = 0;
		xAxis [0,2] = 0;
		xAxis [1,0] = 0;
		xAxis [1,1] = Mathf.Cos(alpha);
		xAxis [1,2] = Mathf.Sin(alpha);
		xAxis [2,0] = 0;
		xAxis [2,1] = -Mathf.Sin(alpha);
		xAxis [2,2] = Mathf.Cos(alpha);
		xAxis [3,3] = 1;
		Matrix4x4 yAxis = new Matrix4x4();
		yAxis [0,0] = Mathf.Cos(beta);
		yAxis [0,1] = 0;
		yAxis [0,2] = -Mathf.Sin(beta);
		yAxis [1,0] = 0;
		yAxis [1,1] = 1;
		yAxis [1,2] = 0;
		yAxis [2,0] = Mathf.Sin(beta);
		yAxis [2,1] = 0;
		yAxis [2,2] = Mathf.Cos(beta);
		yAxis [3,3] = 1;
		Matrix4x4 zAxis = new Matrix4x4();
		zAxis [0,0] = Mathf.Cos(gamma);
		zAxis [0,1] = Mathf.Sin(gamma);
		zAxis [0,2] = 0;
		zAxis [1,0] = -Mathf.Sin(gamma);
		zAxis [1,1] = Mathf.Cos(gamma);
		zAxis [1,2] = 0;
		zAxis [2,0] = 0;
		zAxis [2,1] = 0;
		zAxis [2,2] = 1;
		zAxis [3,3] = 1;
		/*transformMat[0,0] = Mathf.Cos(alpha)*Mathf.Cos(beta);
		transformMat[0,1] = Mathf.Cos(alpha)*Mathf.Sin(beta)*Mathf.Sin(gamma)
			-Mathf.Sin(alpha)*Mathf.Cos(gamma);
		transformMat[0,2] = Mathf.Cos(alpha)*Mathf.Sin(beta)*Mathf.Cos(gamma)
			+Mathf.Sin(alpha)*Mathf.Sin(gamma);
		transformMat[1,0] = Mathf.Sin(alpha)*Mathf.Cos(beta);
		transformMat[1,1] = Mathf.Sin(alpha)*Mathf.Sin(beta)*Mathf.Sin(gamma)
			+Mathf.Cos(alpha)*Mathf.Cos(gamma);
		transformMat[1,2] = Mathf.Sin(alpha)*Mathf.Sin(beta)*Mathf.Cos(gamma)
			-Mathf.Cos(alpha)*Mathf.Sin(gamma);
		transformMat[2,0] = -Mathf.Sin(beta);
		transformMat[2,1] = Mathf.Cos(beta)*Mathf.Sin(gamma);
		transformMat[2,2] = Mathf.Cos(beta)*Mathf.Cos(gamma);
		transformMat[3,3] = 1;*/
		transformMat = zAxis*yAxis*zAxis;
		return transformMat;
	}

	public Chain CreateKinematicChain(RobotLink root, Chain currChain, int forceToChild = -1) {
		//Debug.LogError(root.name);
		IKSolverFABRIK solver = null;
		if (root != _root) {
			FABRIK fabrik = root.gameObject.AddComponent<FABRIK>() as FABRIK;
			_fabriks.Add(fabrik);
			solver = fabrik.GetIKSolver() as IKSolverFABRIK;
		}
		RobotLink parent = root;
		List<RobotLink> progeny = new List<RobotLink>();
		progeny.Add(parent);
		bool looping = true;
		while (looping) {
			List<RobotLink> children = parent.children;
			if(children.Count == 0) {
				looping = false;
				//progeny.Add(child);
				//Debug.LogError (child.name);
			} else if (children.Count == 1 || forceToChild != -1){
				if (forceToChild == -1) {
					parent = children[0];
					progeny.Add(parent);
				} else {
					parent = children[forceToChild];
					progeny.Add(parent);
					forceToChild = -1;
				}
			} else if (children.Count > 1){
				//currChain.childrenChains = CreateKinem1aticChain(child);
				//child = children[0];
				//Rigidbody rb1 = child.gameObject.AddComponent<Rigidbody>() as Rigidbody;
				//rb1.useGravity = false;
				for (int j = 0; j< children.Count; j++) {
					GameObject chainBranch = new GameObject();
					chainBranch.transform.parent = gameObject.transform;
					//progeny.Add(child); //TODO
					chainBranch.gameObject.name = parent.name + j;
					RobotLink newRoot = chainBranch.AddComponent<RobotLink>() as RobotLink;
					newRoot.parent = parent;//.parent;
					//Rigidbody rb2 = newRoot.gameObject.AddComponent<Rigidbody>() as Rigidbody;
					//rb2.useGravity = false;
					//FixedJoint fJ = rb1.gameObject.AddComponent<FixedJoint> () as FixedJoint;
					//fJ.connectedBody = rb2;
					newRoot.gameObject.transform.position = parent.gameObject.transform.position;
					RotationLimitHinge prevRLH = children[j].rotationLimit;//child.gameObject.GetComponent<RotationLimitHinge>() as RotationLimitHinge;
					//Debug.LogError(child);
					if ( prevRLH != null) {
						RotationLimitHinge currrRLH =  newRoot.gameObject.AddComponent<RotationLimitHinge>() as RotationLimitHinge;
						currrRLH.min = prevRLH.min;
						currrRLH.max = prevRLH.max;
						currrRLH.axis = prevRLH.axis;
					}
					//newRoot.gameObject.transform.parent = child.gameObject.transform;

					List<RobotLink> newRootChildren = new List<RobotLink>();
					newRootChildren.Add(children[j]);
					newRoot.children = newRootChildren;
					children[j].gameObject.transform.parent = newRoot.gameObject.transform;
					Chain newChain = new Chain();
					newChain.rootLink = newRoot;
					//if (currChain!=null){
						_numChains++;
					//}
					newChain.parentChain = currChain;
					currChain.childrenChains.Add(newChain);
					//}
					CreateKinematicChain(newRoot,newChain);
				}
				looping = false;
			}
		}
		if (root != _root) {
			solver.bones = new IKSolver.Bone[progeny.Count];
			for (int j =0; j < progeny.Count; j++) {
				solver.bones[j] = new IKSolver.Bone(progeny[j].gameObject.transform);
			}
			/*Vector3 endEffectorPosition = progeny[progeny.Count-1].gameObject.transform.position;
			Transform targetTransform = GameObject.Instantiate(URDFReader.Instance.endEffectorPrefab,
			                                                   endEffectorPosition,
			                                                   Quaternion.identity) as Transform;
			targetTransform.name = root.name + " end effector";
			//solver.target = targetTransform;
			targetTransform.parent = gameObject.transform;*/
		}
		return currChain;
	}

	public void LinkMultipleKinematicChains (Chain rootChain) {
		FABRIKChain newChain = new FABRIKChain();
		if (rootChain.rootLink != null) {
			FABRIK newChainIK = rootChain.rootLink.GetComponent<FABRIK>() as FABRIK;
			if (newChainIK != null) {
				newChain.ik = newChainIK;
				newChain.pin = 0.0f;
				newChain.pull = 1.0f;
				_FABRIKChain.Add (newChain);
				rootChain.index = _currChainIndex;
				_chainDictionary.Add(_currChainIndex,rootChain);
				_currChainIndex++;
			}
		}
		List<Chain> childrenChains = rootChain.childrenChains;
		for (int i = 0; i < childrenChains.Count; i++) {
			//FABRIKChain newChain = new FABRIKChain();
			//newChain.ik = childrenChains[i].rootLink.GetComponent<FABRIK>() as FABRIK;
			//newChain.Initiate();
			//newChain.children
			//_FABRIKChain.Add (newChain);
			LinkMultipleKinematicChains(childrenChains[i]);
		}
	}
}
