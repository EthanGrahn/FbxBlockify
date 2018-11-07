using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
	private CinemachineFreeLook cm;
	
	// Use this for initialization
	void Start ()
	{
		cm = GetComponent<CinemachineFreeLook>();
		//cm.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButton(0))
		{
			cm.m_XAxis.m_InputAxisName = "Mouse X";
			cm.m_YAxis.m_InputAxisName = "Mouse Y";
		}
		else
		{
			cm.m_XAxis.m_InputAxisName = "";
			cm.m_YAxis.m_InputAxisName = "";
		}
	}
}
