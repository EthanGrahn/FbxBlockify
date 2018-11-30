using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour {

	public enum BlockResolution
	{
		EIGHT = 8,
		SIXTEEN = 16,
		THIRTY_TWO = 32,
		SIXTY_FOUR = 64,
		ONE_TWENTY_EIGHT = 128,
		TWO_FIFTY_SIX = 256
	}

	public BlockResolution blockResolution = BlockResolution.SIXTEEN;
	public GameObject model;

	private Button prevButton;
	private bool processing = false;
	private MeshBlockify mBlockify;

	void Start()
	{
		prevButton = GameObject.Find("Res16").GetComponent<Button>();
	}
	
	public void BlockifyObject()
	{
		if (!processing)
		{
			GameObject.Find("BlockifyButton").GetComponent<Button>().interactable = false;
			processing = true;
			if (!(mBlockify = model.GetComponent<MeshBlockify>()))
				mBlockify = model.AddComponent<MeshBlockify>();

			mBlockify.StartBlockify((int) blockResolution);
		}
	}

	public void ChooseResolution(Button button)
	{
		button.interactable = false;
		prevButton.interactable = true;
		prevButton = button;
		blockResolution = (BlockResolution)int.Parse(button.name.Substring(3));
	}

	public void StopProcessing()
	{
		mBlockify.reset = false;
		mBlockify.pause = false;
		processing = false;
		GameObject.Find("BlockifyButton").GetComponent<Button>().interactable = true;
	}

	public void TogglePause()
	{
		if (mBlockify != null)
		{
			mBlockify.pause = !mBlockify.pause;

			if (mBlockify.pause)
				GameObject.Find("PauseText").GetComponent<TextMeshProUGUI>().text = "Continue";
			else
				GameObject.Find("PauseText").GetComponent<TextMeshProUGUI>().text = "Pause";
		}
	}

	public void Reset()
	{
		if (mBlockify != null)
		{
			ProgressBar.Instance.Reset();
			GameObject.Find("PauseText").GetComponent<TextMeshProUGUI>().text = "Pause";
			mBlockify.reset = true;
			CombineUtility.Instance.Reset();
		}
	}
}
