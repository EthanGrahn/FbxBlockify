using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombineUtility : MonoBehaviour
{
	public static CombineUtility Instance;
	
	public MeshFilter helperCubeMesh;

	private MeshFilter m_MeshFilter;
	
	// Use this for initialization
	void Start ()
	{
		Instance = this;
		
		m_MeshFilter = GetComponent<MeshFilter>();
		m_MeshFilter.mesh.name = "none";
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
		if (m_MeshFilter.mesh.name == "none")
			m_MeshFilter.mesh = null;
			
		for (int i = 0; i < transform.childCount; ++i)
			transform.GetChild(i).position += transform.position;
		
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
   
		var meshFilters = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];//-1];
		
		int index = 0;
		for (int i = 0; i < meshFilters.Length; i++)
		{
			if (meshFilters[i].sharedMesh == null) continue;
			
			combine[index].mesh = meshFilters[i].sharedMesh;
			combine[index++].transform = meshFilters[i].transform.localToWorldMatrix;
			//meshFilters[i].GetComponent<Renderer>().enabled = false;
		}
		
		m_MeshFilter.mesh = new Mesh();
		m_MeshFilter.mesh.CombineMeshes (combine);
		GetComponent<Renderer>().material = meshFilters[1].GetComponent<Renderer>().sharedMaterial;
	}

	public void AddCubeAtPosition(Vector3 pos, Color color)
	{
		List<Color> colors = Enumerable.Repeat(color, helperCubeMesh.mesh.vertices.Length).ToList();
		helperCubeMesh.mesh.SetColors(colors);

		helperCubeMesh.gameObject.transform.position = pos;
		CombineChildren();
	}

	public void SetCubeSize(float scale)
	{
		helperCubeMesh.transform.localScale = Vector3.one * scale;
	}
}
