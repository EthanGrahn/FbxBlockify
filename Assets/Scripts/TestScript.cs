using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	public GameObject[] objsToCombine;
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.L))
		{
			CombineChildren();
		}
	}

	void CombineChildren()
	{
		CombineInstance[] combine = new CombineInstance[2];
		
		combine[0].mesh = objsToCombine[0].GetComponent<MeshFilter>().mesh;
		combine[0].transform = gameObject.transform.localToWorldMatrix;
		
		combine[1].mesh = objsToCombine[1].GetComponent<MeshFilter>().mesh;
		combine[1].transform = gameObject.transform.localToWorldMatrix;
		
		Mesh combinedMesh = new Mesh();
		combinedMesh.CombineMeshes(combine, true, false, false);
		MeshFilter mf = gameObject.AddComponent<MeshFilter>();
		mf.mesh = combinedMesh;
	}
}
