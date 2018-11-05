using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollisionEvent : UnityEvent<Collision>
{
}

public class TestSphere : MonoBehaviour
{

	public CollisionEvent collisionEvent;

	private void Start()
	{
		collisionEvent = new CollisionEvent();
	}

	private void OnCollisionEnter(Collision other)
	{
		collisionEvent.Invoke(other);
	}
}
