using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverSupport : MonoBehaviour {

	// Use this for initialization
	public Button mMenu;
	public Button mRestart;
	public Button mRespawn;

	// Use this for initialization
	void Start () {
		// Workflow assume:
		//      mLevelOneButton: is dragged/placed from UI
		// add in listener
		mRestart.onClick.AddListener(
			() => {                     // Lamda operator: define an annoymous function
				LoadScene(SceneManager.GetActiveScene().name);
			});
		mMenu.onClick.AddListener(
			() => {                     // Lamda operator: define an annoymous function
				LoadScene("Menu");
			});
		mRespawn.onClick.AddListener(
			() => {                     // Lamda operator: define an annoymous function
				Hero_Interaction meemo = GameObject.Find("Meemo").GetComponent<Hero_Interaction>();
				meemo.current_state = Hero_Interaction.MeemoState.Respawn;
			});
	}

	// Update is called once per frame
	void Update () {

	}

	void LoadScene(string theLevel) {
		SceneManager.LoadScene(theLevel);
	}
}
