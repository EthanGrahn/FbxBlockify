using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public GameObject blockPrefab;
	public GameObject model;
	
	private List<Block> blocks = new List<Block>();
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(LoadBlocks());
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			BlockifyObject(model);
	}

	private IEnumerator LoadBlocks()
	{
		yield return new WaitForFixedUpdate();
		for (int i = 0; i < 4096; ++i)
		{
			blocks.Add(Instantiate(blockPrefab).transform.GetChild(0).GetComponent<Block>());
			blocks[i].transform.parent.gameObject.SetActive(false);
		}
	}
	
	public void BlockifyObject(GameObject obj)
	{
		MeshBlockify mBlockify;
		if (!(mBlockify = obj.GetComponent<MeshBlockify>()))
			mBlockify = obj.AddComponent<MeshBlockify>();
		
		mBlockify.StartBlockify((int)blockResolution, blocks);
	}
}
