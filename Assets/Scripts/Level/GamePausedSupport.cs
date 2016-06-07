using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePausedSupport : MonoBehaviour
{
    public GameObject PausedPanel;
    private bool escPressed = false;
    public Button mResume;
    public Button mRestart;
    public Button mExit;
	public Button mMenu;

    // Use this for initialization
    void Start()
    {
        PausedPanel.SetActive(false);
        // Workflow assume:
        //      mLevelOneButton: is dragged/placed from UI
        // add in listener
        mRestart.onClick.AddListener(
            () => {                     // Lamda operator: define an annoymous function
                LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1;
            });
        mResume.onClick.AddListener(
            () => {                     // Lamda operator: define an annoymous function
                PausedPanel.SetActive(false);
                Time.timeScale = 1;
            });
        mExit.onClick.AddListener(
            () => {                     // Lamda operator: define an annoymous function
                Application.Quit();
            });
		mMenu.onClick.AddListener(
			() => {                     // Lamda operator: define an annoymous function
				LoadScene("Menu");
			});
    }

    // Update is called once per frame
    void Update()
    {
        Canvas gameOverCanvas = GameObject.Find("GameOverCanvas").GetComponent<Canvas>();
        Canvas gameWinCanvas = GameObject.Find("GameWinCanvas").GetComponent<Canvas>();
        Debug.Log(!gameOverCanvas.enabled + " or " + !gameWinCanvas.enabled);
        if (!gameOverCanvas.enabled && !gameWinCanvas.enabled)
        {
            //Debug.Log(gameOverCanvas.enabled + " or " + gameWinCanvas.enabled);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                escPressed = !escPressed;
                if (escPressed)
                {
                    PausedPanel.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    PausedPanel.SetActive(false);
                    Time.timeScale = 1;
                }
            }
        }

    }

    void LoadScene(string theLevel)
    {
        SceneManager.LoadScene(theLevel);
    }
}