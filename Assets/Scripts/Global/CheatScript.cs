using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CheatScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1"))
			SceneManager.LoadScene ("Jump");
		if (Input.GetKeyDown ("2"))
			SceneManager.LoadScene ("Level2Scene");
		if (Input.GetKeyDown ("j"))
			GameObject.Find ("Meemo").GetComponent<Hero_Interaction> ().is_jetpack_man = true;
		if (Input.GetKeyDown ("k"))
			GameObject.Find ("Meemo").GetComponent<Hero_Interaction> ().is_invincible = true;
		
	}	
}
