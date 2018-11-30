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

	private Vector3 prevPosition = Vector3.negativeInfinity;

	private void Start()
	{
		collisionEvent = new BlockEvent();
	}

	private void OnCollisionEnter(Collision other)
	{
		collisionEvent.Invoke(other, this);
	}

	private void OnCollisionStay(Collision other)
	{
		if (transform.position != prevPosition)
		{
			prevPosition = transform.position;
			collisionEvent.Invoke(other, this);
		}
	}

	public void SetSize(float size)
	{
		transform.localScale = Vector3.one * size;
	}

	public void SetPosition(Vector3 position)
	{
		transform.parent.position = position;
	}
}
