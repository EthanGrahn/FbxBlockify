using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDebug : MonoBehaviour
{
	private Mesh mesh;
	public Color[] colors;
	
	// Use this for initialization
	void Start ()
	{
		mesh = GetComponent<MeshFilter>().mesh;
		colors = new []{((Texture2D) (GetComponent<MeshRenderer>().materials[2].mainTexture)).GetPixelBilinear(mesh.uv[10].x, mesh.uv[10].y)};
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
			GetColors();
	}

	void GetColors()
	{
		colors = mesh.colors;
	}
}
