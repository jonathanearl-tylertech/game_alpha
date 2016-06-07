using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndCliffCollider : MonoBehaviour {
    private Canvas gameWinCanvas;
    // Use this for initialization
    void Start()
    {
        gameWinCanvas = GameObject.Find("GameWinCanvas").GetComponent<Canvas>();
        gameWinCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
			other.gameObject.GetComponent<Hero_Interaction> ().current_state = Hero_Interaction.MeemoState.Dead;
            // Code to handle electricity and bouncing out
            gameWinCanvas.enabled = true;
        }
    }
}
