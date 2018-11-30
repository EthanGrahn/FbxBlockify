using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViewHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public static bool MouseOver = false;
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		MouseOver = true;
	}
 
	public void OnPointerExit(PointerEventData eventData)
	{
		MouseOver = false;
	}
}
