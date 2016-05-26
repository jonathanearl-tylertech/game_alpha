using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndCliffCollider : MonoBehaviour {
    public GameObject winningPanel = null;
    // Use this for initialization
    void Start()
    {
		winningPanel.SetActive (false);
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
            winningPanel.SetActive(true);
        }
    }
}
