using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Globalization;

public class MoCapReader : MonoBehaviour {
	
	public string fileName;
	public int frameDensity = 15;
	public Dictionary<string,List<Vector3>> effectorPositions;
	public static MoCapReader instance;
	
	
	public static MoCapReader Instance
	{
		get 
		{
			return instance;
		}
	}
	// Use this for initialization
	void Awake () {
		instance = this;
		using (StreamReader sr = new StreamReader(fileName))
		{
			int framesUsed = 0;
			sr.ReadLine();
			sr.ReadLine();
			sr.ReadLine();
			string currJoint = "";
			List<Vector3> currTargets = new List<Vector3>();
			effectorPositions = new Dictionary<string, List<Vector3>>();
			while (!sr.EndOfStream) {
				string[] line = sr.ReadLine().Split( new char[] { ' ' } );
				float xPosition;
				float yPosition;
				float zPosition;
				//framesUsed++;
				if(float.TryParse(line[0],out xPosition)){
					if (framesUsed%frameDensity==0) {
						float.TryParse(line[1],out yPosition);
						float.TryParse(line[2],out zPosition);
						Vector3 position = new Vector3(xPosition, yPosition, zPosition);
						currTargets.Add(position);
					}
					//Debug.LogError(position);
				} else {
					if (line.Length>1){
						if(!String.IsNullOrEmpty(currJoint)){
							effectorPositions.Add(currJoint,currTargets);
						}
						currJoint = line[1];
						currTargets = new List<Vector3>();
						Debug.LogError(line[1]);
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
