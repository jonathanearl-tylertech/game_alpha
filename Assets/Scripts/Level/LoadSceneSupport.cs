using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// for SceneManager

public class LoadSceneSupport : MonoBehaviour {

	public string LevelName = null;

    public Button mStart;
    public Button mExit;
    public Button mCredit;
    public Canvas creditCanvas;

	// Use this for initialization
	void Start () {
        creditCanvas.enabled = false;
        // Workflow assume:
        //      m-Buttons: are dragged/placed from UI
        // add in listener
        mStart.onClick.AddListener(
                () => {                     // Lamda operator: define an annoymous function
                LoadScene("Jump");
                });
        mExit.onClick.AddListener(
                () => {                     // Lamda operator: define an annoymous function
                    Application.Quit();
                });
        mCredit.onClick.AddListener(
                () => {                     // Lamda operator: define an annoymous function
                    creditCanvas.enabled = true;
                });

    }

	public void gotoMainMenu(){
		LoadScene ("Menu");
	}

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(creditCanvas.enabled == true)
            {
                creditCanvas.enabled = false;
            }
        }
    }
    
	void LoadScene(string theLevel) {
        SceneManager.LoadScene(theLevel);
	}
}