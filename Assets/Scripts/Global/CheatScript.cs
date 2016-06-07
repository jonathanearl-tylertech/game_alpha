using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CheatScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("0"))
			SceneManager.LoadScene ("TutorialScene");
		if (Input.GetKeyDown ("1"))
			SceneManager.LoadScene ("Jump");
		if (Input.GetKeyDown ("2"))
			SceneManager.LoadScene ("Level2Scene");
		if (Input.GetKeyDown ("j")) {
			Hero_Interaction h_i = GameObject.Find ("Meemo").GetComponent<Hero_Interaction> ();
			h_i.is_jetpack_man = !h_i.is_jetpack_man;
		}
		if (Input.GetKeyDown ("k")) {
			Hero_Interaction h_i = GameObject.Find ("Meemo").GetComponent<Hero_Interaction> ();
			h_i.is_invincible = !h_i.is_invincible;
		}
	}	
}
