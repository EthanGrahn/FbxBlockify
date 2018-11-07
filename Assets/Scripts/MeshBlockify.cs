using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshBlockify : MonoBehaviour
{
	private Mesh mesh;
	public GameObject testObject;

	private Texture2D m_MainTexture;
	private MeshCollider _collider;
	private MeshRenderer _renderer;
	private List<Block> blocks = new List<Block>();
	
	// Use this for initialization
	void Start ()
	{
		mesh = GetComponent<MeshFilter>().mesh;
		_collider = GetComponent<MeshCollider>();
		_renderer = GetComponent<MeshRenderer>();

		GameObject.Find("CameraFocus").transform.position = _renderer.bounds.center;
		
		
		testObject.GetComponent<Block>().collisionEvent.AddListener(ObjectCollide);
		m_MainTexture = _renderer.material.mainTexture as Texture2D;
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
		for (int i = 0; i < locations.Length; ++i)
		{
			blocks[i].transform.parent.gameObject.SetActive(true);
			yield return new WaitForFixedUpdate();
			blocks[i].collisionEvent.AddListener(ObjectCollide);
			blocks[i].SetSize(testObjectSize);
			blocks[i].SetPosition(locations[i]);
		}
	}

	private void ObjectCollide(Collision other, Block block)
	{
		List<Color> colorList = new List<Color>();
		foreach(ContactPoint cp in other.contacts)
		{
			RaycastHit hit;
			float rayLength = 0.1f;
			Ray ray = new Ray(cp.point - cp.normal * rayLength * 0.5f, cp.normal);
			//Debug.DrawRay(cp.point - cp.normal * rayLength, cp.normal, Color.red, 50, false);
			if (_collider.Raycast(ray, out hit, rayLength))
			{
				colorList.Add(m_MainTexture.GetPixelBilinear(hit.textureCoord.x, hit.textureCoord.y));
			}
		}

		block.SetColor(AverageColors(colorList));
	}

	public Vector3[] SubdivideBounds(float objSize, Bounds bounds, float cubeExtent, int depth)
	{
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
