using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MeshDebug : MonoBehaviour
{
	private Mesh mesh;
	public GameObject testObject;

	private Texture2D m_MainTexture;
	private Color vertexColor = Color.black;
	
	// Use this for initialization
	void Start ()
	{
		mesh = GetComponent<MeshFilter>().mesh;
		testObject.GetComponent<TestSphere>().collisionEvent.AddListener(SphereCollide);
		m_MainTexture = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
	}

	private async void TestPoints()
	{
		Vector3[] verts = mesh.vertices;

		for (int i = 0; i < verts.Length; ++i)
		{
			testObject.transform.position = new Vector3(verts[i].x, verts[i].y, verts[i].z);
			await Task.Delay(5);
		}
	}

	private void SphereCollide(Collision other)
	{
		List<Color> colorList = new List<Color>();
		foreach(ContactPoint cp in other.contacts)
		{
			RaycastHit hit;
			float rayLength = 0.1f;
			Ray ray = new Ray(cp.point + cp.normal * rayLength * 0.5f, -cp.normal);
			if (cp.thisCollider.Raycast(ray, out hit, rayLength))
			{
				colorList.Add(m_MainTexture.GetPixelBilinear(hit.textureCoord.x, hit.textureCoord.y));
			}
		}

		vertexColor = AverageColors(colorList);
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
