using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Globalization;

public class URDFReader : MonoBehaviour
{
	public RobotLink linkPrefab;
	public RobotJoint jointPrefab;
	public Robot robotPrefab;
	public Transform endEffectorPrefab;
	private List<string> links;
	[HideInInspector] public Dictionary<string,RobotLink> RobotLinks;
	[HideInInspector] public Dictionary<string,RobotJoint> RobotJoints;
	public string fileName;
	public string meshFolderName;
	private int numLinks = 0;
	public Robot robot;
	public static URDFReader instance;
	private char[] ignoreCharacters;
	private List<string> meshNames;
	
	
	public static URDFReader Instance
	{
		get 
		{
			return instance;
		}
	}
	void Awake () {
		instance = this;
	}
	void Start ()
	{
		RobotLinks = new Dictionary<string, RobotLink>();
		RobotJoints = new Dictionary<string, RobotJoint>();
		meshNames = new List<string>();
		ignoreCharacters = new char[] { ' ', '<', '>', '\"'};
		//try
		//{
			using (StreamReader sr = new StreamReader(fileName))
			{
				while (!sr.EndOfStream) {
					string[] line = sr.ReadLine().Split(ignoreCharacters );
					// Get block
				List<List<string>> block;// = new List<List<string>>();
					for (int i = 0;i < line.Length; i++) {
						if (line[i].Equals("link")) {
							numLinks++;
							block = GetBlock (line, sr,"link");
							ProcessRobotLink(block);
						} else if (line[i].Equals("joint")) {
							block = GetBlock (line, sr,"joint");
							//ProcessRobotJoint(block);
						} else if (line[i].Equals("transmission")) {
							block = GetBlock (line, sr,"transmission");
							//ProcessTransmission(block);
						}
					}
				}
			}
		//}
		/*catch (Exception e)
		{
			Debug.LogError("The file could not be read:");
			Debug.LogError(e.Message);
		}*/
		//try
		//{
			using (StreamReader sr = new StreamReader(fileName))
			{
				while (!sr.EndOfStream) {
					string[] line = sr.ReadLine().Split(ignoreCharacters);
					// Get block
					List<List<string>> block;// = new List<List<string>>();
					for (int i = 0;i < line.Length; i++) {
						if (line[i].Equals("link")) {
							block = GetBlock (line, sr,"link");
							//ProcessRobotLink(block);
						} else if (line[i].Equals("joint")) {
							block = GetBlock (line, sr,"joint");
							ProcessRobotJoint(block);
						} else if (line[i].Equals("transmission")) {
							block = GetBlock (line, sr,"transmission");
							//ProcessTransmission(block);
						}
					}
				}
			}
		/*}
		catch (Exception e)
		{
			Debug.LogError("The file could not be read:");
			Debug.LogError(e.Message);
		}*/
		//Robot robot = GameObject.Instantiate(robotPrefab,new Vector3(),Quaternion.identity) as Robot;
		robot.BuildRobot(RobotLinks,RobotJoints);
	}

	public List<List<string>> GetBlock (string[] line, StreamReader sr, string killWord) {
		List<List<string>> block = new List<List<string>>();
		for (int i =0; i < line.Length; i++) {
			block.Add(new List<string>());
			if(line[i].Equals("/")) {
				return block;
			}
			block[0].Add(line[i]);
		}
		bool blockOpen = true;
		while (blockOpen && !sr.EndOfStream) {
			line = sr.ReadLine().Split(ignoreCharacters);
			block.Add(new List<string>());
			for (int i =0; i < line.Length; i++) {
				if(line[i].Contains("/"+killWord)) {
					return block;
				}
				block[block.Count-1].Add(line[i]);
			}
		}
		return block;


	}
	public void ProcessRobotLink (List<List<string>> block){
		string linkName = "";
		for (int i = 0;i < block.Count; i++) {
			for(int j = 0; j < block[i].Count; j++) {
				if (!block[i][j].Equals("")){
					if (block[i][j].Equals("name=") && block[i][j-1].Equals("link")){
						linkName = block[i][j+1];
						if (!RobotLinks.ContainsKey(linkName))
						{
							RobotLink newRobotLink = GameObject.Instantiate(linkPrefab,new Vector3(),Quaternion.identity) as RobotLink;
							newRobotLink.transform.parent = robot.transform;
							newRobotLink.gameObject.name = linkName;
							RobotLinks.Add(linkName,newRobotLink);
						}
						j++;
					} else if (block[i][j].Contains(meshFolderName)){
						string meshName = block[i][j];
						string[] meshNameSplit = meshName.Split(new char[] { '.','/' });
						meshName = meshNameSplit[1];
						if(!meshNames.Contains(meshName))
						{
							meshNames.Add (meshName);
							meshName = meshFolderName + "/" + meshName;
							//Debug.LogError (meshName);
							GameObject mesh = GameObject.Instantiate(Resources.Load(meshName)) as GameObject;
							mesh.gameObject.transform.parent = RobotLinks[linkName].gameObject.transform;
							RobotLinks[linkName].mesh = mesh;
						}
						j++;
					}
				}
			}
		}
	}
	
	public void ProcessRobotJoint (List<List<string>> block){
		string jointName = "";
		for (int i = 0;i < block.Count; i++) {
			for (int j = 0; j <block[i].Count; j++) {
				if (!block[i][j].Equals("")){
					if (block[i][j].Equals("name=")){
						RobotJoint newRobotJoint = GameObject.Instantiate(jointPrefab,new Vector3(),Quaternion.identity) as RobotJoint;
						newRobotJoint.transform.parent = robot.transform;
						newRobotJoint.gameObject.name = block[i][j+1];
						//Debug.LogError(block[i][j+1]);
						RobotJoints.Add(block[i][j+1],newRobotJoint);
						jointName = block[i][j+1];
						j++;
					}else if (block[i][j].Contains("origin")){
						AddOrigin (block[i],RobotJoints[jointName]);
						break;
					} else if (block[i][j].Contains("axis")){
						AddAxis (block[i],RobotJoints[jointName]);
						break;
					} else if (block[i][j].Contains("parent")){
						j++;
						j++;
						RobotJoints[jointName].parent = RobotLinks[block[i][j]];
						break;
					} else if (block[i][j].Contains("child")){
						j++;
						j++;
						//Debug.LogError(block[i][j]);
						RobotLink childLink;
						if(RobotLinks.TryGetValue(block[i][j],out childLink))
						{
							RobotJoints[jointName].child = RobotLinks[block[i][j]];
						}
						break;
					}else if (block[i][j].Contains("lower=")){
						j++;
						//j++;
						RobotJoints[jointName].limitMin = float.Parse(block[i][j]);
						j++;
						j++;
						j++;
						RobotJoints[jointName].limitMax = float.Parse(block[i][j]);
						break;
						break;
					}
				}
			}
		}
	}

	public void AddOrigin (List<string> line, RobotPart part) {
		for (int i = 0; i < line.Count; i++) {
			if (line[i].Contains("xyz")){
				Vector3 xyz = new Vector3();
				i++;
				xyz.x = (float)decimal.Parse(line[i]);
				i++;
				xyz.y = (float)decimal.Parse(line[i]);
				i++;
				xyz.z = (float)decimal.Parse(line[i]);
				part.originXYZ = xyz;
			} else if (line[i].Contains("rpy")){
				Vector3 rpy = new Vector3();
				i++;
				rpy.x = Mathf.Rad2Deg*(float)decimal.Parse(line[i]);
				i++;
				rpy.y = Mathf.Rad2Deg*(float)decimal.Parse(line[i]);
				i++;
				rpy.z = Mathf.Rad2Deg*(float)decimal.Parse(line[i]);
				part.originRPY = rpy;
			}
		}
	}

	public void AddAxis (List<string> line, RobotPart part) {
		for (int i = 0; i < line.Count; i++) {
			if (line[i].Contains("xyz")){
				Vector3 xyz = new Vector3();
				i++;
				xyz.x = (float)decimal.Parse(line[i]);
				i++;
				xyz.y = (float)decimal.Parse(line[i]);
				i++;
				xyz.z = (float)decimal.Parse(line[i]);
				part.axisXYZ = xyz;
			}
		}
	}
}