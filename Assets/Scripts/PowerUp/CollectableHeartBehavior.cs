using UnityEngine;
using System.Collections;

public class CollectableHeartBehavior : MonoBehaviour {
	private AudioSource collectingSound;

	// Use this for initialization
	void Start () {
		collectingSound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
