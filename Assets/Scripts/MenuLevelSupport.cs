using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// for SceneManager

public class MenuLevelSupport : MonoBehaviour {

	public string LevelName = null;

	public Button mMenu;
	public Button mRestart;


	// Use this for initialization
	void Start () {
		// Workflow assume:
		//      mLevelOneButton: is dragged/placed from UI
		// add in listener
		mRestart.onClick.AddListener(
			() => {                     // Lamda operator: define an annoymous function
				if(Time.timeScale == 0) Time.timeScale = 1;
				LoadScene("Jump");
			});
		mMenu.onClick.AddListener(
			() => {                     // Lamda operator: define an annoymous function
				if(Time.timeScale == 0) Time.timeScale = 1;
				LoadScene("Menu");
			});

	}
		
	// Update is called once per frame
	void Update () {

	}

	void LoadScene(string theLevel) {
		SceneManager.LoadScene(theLevel);
	}
}
