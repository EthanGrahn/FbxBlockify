using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadObj(string fileLocation)
	{
		Mesh holderMesh = new Mesh();
		ObjImporter newMesh = new ObjImporter();
		holderMesh = newMesh.ImportFile(fileLocation);

		MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
		MeshFilter filter = gameObject.AddComponent<MeshFilter>();
		filter.mesh = holderMesh;
	}
}
