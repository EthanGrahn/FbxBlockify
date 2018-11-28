using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshBlockify : MonoBehaviour
{
	private struct SubCube
	{
		public SubCube(MeshFilter m, Vector3 c)
		{
			mFilter = m;
			center = c;
		}
		
		public MeshFilter mFilter;
		public Vector3 center;

		public void SetPosition(Vector3 newPosition)
		{
			List<Vector3> newVerts = new List<Vector3>();
			newVerts.AddRange(mFilter.mesh.vertices);
			for (int i = 0; i < mFilter.mesh.vertices.Length; ++i)
			{
				newVerts.Add(mFilter.mesh.vertices[i] + (center - newPosition));
			}
			
			mFilter.mesh.SetVertices(newVerts);
			center = newPosition;
		}
	}
	
	private MeshFilter mFilter;
	public GameObject testObject;

	private Texture2D m_MainTexture;
	private MeshCollider _collider;
	private MeshRenderer _renderer;
	private List<Block> blocks = new List<Block>();
	private List<SubCube> meshCubes = new List<SubCube>();
	private SubCube baseCube;
	
	// Use this for initialization
	void Start ()
	{
		_collider = GetComponent<MeshCollider>();
		_renderer = GetComponent<MeshRenderer>();

		GameObject.Find("CameraFocus").transform.position = _renderer.bounds.center;

		MeshFilter _mFilter = testObject.GetComponent<MeshFilter>();
		mFilter = _mFilter;
		Vector3 _center = testObject.GetComponent<MeshRenderer>().bounds.center;
		baseCube = new SubCube(_mFilter, _center);
		//meshCubes.Add(baseCube);
		//AddToMesh(baseCube.mFilter);
		
		
		//testObject.GetComponent<Block>().collisionEvent.AddListener(ObjectCollide);
		m_MainTexture = _renderer.material.mainTexture as Texture2D;
		if (m_MainTexture == null)
			Debug.Log("main texture is null");
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			baseCube = new SubCube(mFilter, Vector3.zero);
			baseCube.SetPosition(testObject.transform.position + Vector3.up);
			AddToMesh(baseCube.mFilter);
		}
	}

	public void StartBlockify(int blockResolution, List<Block> blockList)
	{
		blocks = blockList;
		StartCoroutine(AssignBlocks(blockResolution));
	}

	private IEnumerator AssignBlocks(int blockResolution)
	{
		float cubeExtents = Mathf.Max(new float[] {_renderer.bounds.extents.x, _renderer.bounds.extents.y, _renderer.bounds.extents.z});
		float testObjectSize = (cubeExtents * 2f) / blockResolution;
		
		Vector3[] locations = SubdivideBounds(testObjectSize, _renderer.bounds, cubeExtents, blockResolution);
		yield return new WaitForFixedUpdate();

		Block previousBlock = null;
		
		for (int i = 0; i < locations.Length; ++i)
		{
			Block block;
			if (previousBlock == null || previousBlock.transform.parent.gameObject.activeInHierarchy)
			{
				block = Instantiate(testObject).transform.GetChild(0).GetComponent<Block>();
				previousBlock = block;
			}
			else
			{
				previousBlock.Activate();
				block = previousBlock;
			}
			//blocks[i].transform.parent.gameObject.SetActive(true);
			yield return new WaitForFixedUpdate();
			block.collisionEvent.AddListener(ObjectCollide);
			block.SetSize(testObjectSize);
			block.SetPosition(locations[i]);
			yield return new WaitForFixedUpdate();
		}
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
		{
			block.gameObject.SetActive(false);
			return;
		}

		block.SetColor(AverageColors(colorList));
	}

	private void AddToMesh(MeshFilter newMesh)
	{
		if (mFilter.sharedMesh == null)
		{
			mFilter = newMesh;
			return;
		}
		
		CombineInstance[] combine = new CombineInstance[2];
		
		combine[0].mesh = mFilter.mesh;
		combine[0].transform = mFilter.gameObject.transform.localToWorldMatrix;
		
		combine[1].mesh = newMesh.mesh;
		combine[1].transform = mFilter.gameObject.transform.localToWorldMatrix;
		
		Mesh combinedMesh = new Mesh();
		combinedMesh.CombineMeshes(combine);
		mFilter.mesh = combinedMesh;
	}

	public Vector3[] SubdivideBounds(float objSize, Bounds bounds, float cubeExtent, int depth)
	{
		Debug.Log(depth);
		List<Vector3> positions = new List<Vector3>();
		float xStart = bounds.center.x - cubeExtent;
		float yStart = bounds.center.y - cubeExtent;
		float zStart = bounds.center.z - cubeExtent;
		float xEnd = bounds.center.x + cubeExtent - objSize;
		float yEnd = bounds.center.y + cubeExtent - objSize;
		float zEnd = bounds.center.z + cubeExtent - objSize;
		
		int count = 0;
		
		for (float x = xStart; x < xEnd; x += objSize)
		{
			for (float y = yStart; y < yEnd; y += objSize)
			{
				for (float z = zStart; z < zEnd; z += objSize)
				{
					positions.Add(new Vector3(x, y, z));
					count++;
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
