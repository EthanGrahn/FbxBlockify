using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
	public static ProgressBar Instance;

	public RectTransform bar;
	public TextMeshProUGUI text;
	
	// Use this for initialization
	void Start ()
	{
		Instance = this;
	}

	public void Reset()
	{
		bar.offsetMax = new Vector2(-716, bar.offsetMax.y);
		text.text = "0%";
	}

	public void UpdateProgress(float p)
	{
		float percent = (p * 716) - 716;
		bar.offsetMax = new Vector2(percent, bar.offsetMax.y);
		text.text = Mathf.RoundToInt(p * 100f).ToString() + "%";
	}
}
