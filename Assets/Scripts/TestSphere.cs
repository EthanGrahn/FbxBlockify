using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestSphere : MonoBehaviour
{

	public UnityEvent<Collision> collisionEvent;

	private void OnCollisionEnter(Collision other)
	{
		collisionEvent.Invoke(other);
	}
}
