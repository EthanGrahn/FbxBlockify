using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshBlockify : MonoBehaviour
{
	private MeshFilter mFilter;
	public GameObject testObject;

	private Texture2D m_MainTexture;
	private MeshCollider _collider;
	private MeshRenderer _renderer;
	public bool pause = false;
	public bool reset = false;
	
	// Use this for initialization
	void Start ()
	{
		_collider = GetComponent<MeshCollider>();
		_renderer = GetComponent<MeshRenderer>();

		GameObject.Find("CameraFocus").transform.position = _renderer.bounds.center;
		
		m_MainTexture = _renderer.material.mainTexture as Texture2D;
		if (m_MainTexture == null)
			Debug.Log("main texture is null");
	}

	public void StartBlockify(int blockResolution)
	{
		StartCoroutine(AssignBlocks(blockResolution));
	}

	private IEnumerator AssignBlocks(int blockResolution)
	{
		float cubeExtents = Mathf.Max(new float[] {_renderer.bounds.extents.x, _renderer.bounds.extents.y, _renderer.bounds.extents.z});
		float testObjectSize = (cubeExtents * 2f) / blockResolution;
		CombineUtility.Instance.SetCubeSize(testObjectSize);
		
		Vector3[] locations = SubdivideBounds(testObjectSize, _renderer.bounds, cubeExtents, blockResolution);

		Block block = Instantiate(testObject).transform.GetChild(0).GetComponent<Block>();
		yield return new WaitForFixedUpdate();
		block.collisionEvent.AddListener(ObjectCollide);
		block.SetSize(testObjectSize);
		
		for (int i = 0; i < locations.Length && !reset; ++i)
		{
			ProgressBar.Instance.UpdateProgress(i / (float)locations.Length);
			block.SetPosition(locations[i]);
			yield return new WaitForFixedUpdate();
			yield return new WaitUntil(() => !pause || reset);
		}
		
		Destroy(block.gameObject);
		GameObject.FindObjectOfType<BlockManager>().StopProcessing();
	}

	private void ObjectCollide(Collision other, Block block)
	{
		List<Color> colorList = new List<Color>();
		foreach(ContactPoint cp in other.contacts)
		{
			RaycastHit hit;
			float rayLength = 0.1f;
			Ray ray = new Ray(cp.point - cp.normal * rayLength, cp.normal);
			//Debug.DrawRay(cp.point - cp.normal * rayLength, cp.normal, Color.red, 50, false);
			if (_collider.Raycast(ray, out hit, rayLength))
			{
				colorList.Add(m_MainTexture.GetPixelBilinear(hit.textureCoord.x, hit.textureCoord.y));
			}
		}

		if (colorList.Count == 0)
			return;

		CombineUtility.Instance.AddCubeAtPosition(block.transform.position, AverageColors(colorList));
	}

	private void AddToMesh(MeshFilter newMesh)
	{
		CombineInstance[] combine = new CombineInstance[2];
		
		combine[0].mesh = mFilter.sharedMesh;
		combine[0].transform = mFilter.gameObject.transform.localToWorldMatrix;
		
		combine[1].mesh = newMesh.sharedMesh;
		combine[1].transform = mFilter.gameObject.transform.localToWorldMatrix;
		
		Mesh combinedMesh = new Mesh();
		combinedMesh.CombineMeshes(combine, false);
		mFilter.sharedMesh = combinedMesh;
	}

	public Vector3[] SubdivideBounds(float objSize, Bounds bounds, float cubeExtent, int depth)
	{
		Debug.Log(depth);
		List<Vector3> positions = new List<Vector3>();
		float xStart = bounds.center.x - cubeExtent - objSize;
		float yStart = bounds.center.y - cubeExtent - objSize;
		float zStart = bounds.center.z - cubeExtent - objSize;
		float xEnd = bounds.center.x + cubeExtent;
		float yEnd = bounds.center.y + cubeExtent;
		float zEnd = bounds.center.z + cubeExtent;
		
		for (float x = xStart; x < xEnd; x += objSize)
		{
			for (float y = yStart; y < yEnd; y += objSize)
			{
				for (float z = zStart; z < zEnd; z += objSize)
				{
					positions.Add(new Vector3(x, y, z));
				}
			}
		}
		
		return positions.ToArray();
	}
	
	private Color AverageColors(List<Color> colors)
	{
		float r = 0;
		float g = 0;
		float b = 0;

		foreach (Color color in colors)
		{
			r += Mathf.Pow(color.r, 2);
			g += Mathf.Pow(color.g, 2);
			b += Mathf.Pow(color.b, 2);
		}

		r = Mathf.Sqrt(r / colors.Count);
		g = Mathf.Sqrt(g / colors.Count);
		b = Mathf.Sqrt(b / colors.Count);
		
		return new Color(r, g, b);
	}
}
