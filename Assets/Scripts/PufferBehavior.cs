using UnityEngine;
using System.Collections;

public class PufferBehavior : MonoBehaviour {
	private float distFromMeemoToActivateTrigger = 5f;

	#region state support
	public enum PufferState
	{
		Little,
		Puffed
	}
	public PufferState currentState;
	#endregion

	// Use this for initialization
	void Start () {
		currentState = PufferState.Little;
	}

	// Update is called once per frame
	void Update () {
		Canvas gameOverCanvas = GameObject.Find ("GameOverCanvas").GetComponent<Canvas> ();
		if (currentState == PufferState.Little && !gameOverCanvas.enabled) {
			Hero_Interaction meemo = GameObject.FindGameObjectWithTag ("Player").GetComponent<Hero_Interaction> ();
			if (Vector3.Distance (meemo.transform.position, transform.position) < distFromMeemoToActivateTrigger) {
				currentState = PufferState.Puffed;
			}
		} else {
			
		}
	}
}
