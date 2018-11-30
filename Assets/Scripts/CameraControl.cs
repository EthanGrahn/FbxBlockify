using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
	private CinemachineFreeLook cm;
	private float sensMultiplier = 1;
	
	// Use this for initialization
	void Start ()
	{
		cm = GetComponent<CinemachineFreeLook>();
		GameObject.Find("ZoomSensSlider").GetComponent<Slider>().onValueChanged.AddListener(SensChanged);
		//cm.enabled = false;
	}

	private void SensChanged(float value)
	{
		sensMultiplier = value;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButton(0) && ViewHelper.MouseOver)
		{
			cm.m_XAxis.m_InputAxisName = "Mouse X";
			cm.m_YAxis.m_InputAxisName = "Mouse Y";
		}
		else
		{
			cm.m_XAxis.m_InputAxisName = "";
			cm.m_YAxis.m_InputAxisName = "";
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			cm.m_Lens.FieldOfView += sensMultiplier;
			cm.m_Lens.FieldOfView = Mathf.Clamp(cm.m_Lens.FieldOfView, 5, 100);
		}
		else if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			cm.m_Lens.FieldOfView -= sensMultiplier;
			cm.m_Lens.FieldOfView = Mathf.Clamp(cm.m_Lens.FieldOfView, 5, 100);
		}
	}
}
