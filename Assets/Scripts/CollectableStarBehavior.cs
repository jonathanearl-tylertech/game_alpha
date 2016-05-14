using UnityEngine;
using System.Collections;

public class CollectableStarBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			// Code
			other.gameObject.GetComponent<Hero_Interaction>().ResetStarPower();
			Debug.Log("Meemo touches star");
			Destroy (this.gameObject);
		}
	}
}
