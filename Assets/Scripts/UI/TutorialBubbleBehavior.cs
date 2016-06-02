using UnityEngine;
using System.Collections;

public class TutorialBubbleBehavior : MonoBehaviour {
	private TuBuState State;

	private enum TuBuState
	{
		Station,
		Moving
	}
	// Use this for initialization
	void Start () {
		State = TuBuState.Station;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			

		}
	}
}
