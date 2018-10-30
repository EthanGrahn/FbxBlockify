using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GracesGames.SimpleFileBrowser.Scripts;

public class FileHandler : MonoBehaviour {

	// Use the file browser prefab
	public GameObject FileBrowserPrefab;
	
	// Use this for initialization
	void Start () {
		//_loadedText = GameObject.Find("LoadedText");

		GameObject uiCanvas = GameObject.Find("Canvas");
		if (uiCanvas == null)
			Debug.LogError("Make sure there is a canvas GameObject present in the hierarchy (Create UI/Canvas)");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// Open the file browser using boolean parameter so it can be called in GUI
	public void OpenFileBrowser(bool saving) {
		OpenFileBrowser(saving ? FileBrowserMode.Save : FileBrowserMode.Load);
	}

	// Open a file browser to save and load files
	private void OpenFileBrowser(FileBrowserMode fileBrowserMode) {
		// Create the file browser and name it
		GameObject fileBrowserObject = Instantiate<GameObject>(FileBrowserPrefab, transform);
		fileBrowserObject.name = "FileBrowser";
		// Set the mode to save or load
		FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
		fileBrowserScript.SetupFileBrowser(ViewMode.Landscape);
		if (fileBrowserMode == FileBrowserMode.Save) {
			//fileBrowserScript.SaveFilePanel("DemoText", FileExtensions);
			// Subscribe to OnFileSelect event (call SaveFileUsingPath using path) 
			//fileBrowserScript.OnFileSelect += SaveFileUsingPath;
		} else {
			fileBrowserScript.OpenFilePanel(new string[] {"obj"});
			// Subscribe to OnFileSelect event (call LoadFileUsingPath using path) 
			fileBrowserScript.OnFileSelect += LoadObj;
		}
	}
	
	public void LoadObj(string fileLocation)
	{
		Mesh holderMesh = new Mesh();
		ObjImporter newMesh = new ObjImporter();
		holderMesh = newMesh.ImportFile(fileLocation);

		MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
		MeshFilter filter = gameObject.AddComponent<MeshFilter>();
		filter.mesh = holderMesh;
	}
}
