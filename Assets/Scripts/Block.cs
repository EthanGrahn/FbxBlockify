using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BlockEvent : UnityEvent<Collision, Block>
{
}

public class Block : MonoBehaviour
{

	public BlockEvent collisionEvent;

	private bool collides = false;

	private void Start()
	{
		collisionEvent = new BlockEvent();
	}

	private void OnCollisionEnter(Collision other)
	{
		//Debug.Log("collision interior");
		collides = true;
		collisionEvent.Invoke(other, this);
	}

	public void SetColor(Color color)
	{
		GetComponent<MeshRenderer>().material.color = color;
	}

	public void SetSize(float size)
	{
		transform.localScale = Vector3.one * size;
	}

	public void SetPosition(Vector3 position)
	{
		transform.parent.position = position;
		StartCoroutine(CheckIfCollide());
	}

	private IEnumerator CheckIfCollide()
	{
		yield return new WaitForFixedUpdate();
		if (!collides)
			transform.parent.gameObject.SetActive(false);
	}
}
