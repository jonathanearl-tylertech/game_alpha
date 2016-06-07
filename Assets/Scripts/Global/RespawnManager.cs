using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour {
	public const int MAX_RESPAWN_TIMES = 3;
	public GameObject[] checkpoints;
	public GameObject latestCheckpoint;
	public Hero_Interaction meemo;
	public Vector3 meemoPosition;


	// Use this for initialization
	void Start () {
		checkpoints = GameObject.FindGameObjectsWithTag ("checkPoint");
		meemo = GameObject.Find ("Meemo").GetComponent<Hero_Interaction> ();
		for (int i = 0; i < checkpoints.Length; i++) {
			Debug.Log ("checkpoint" + i + " " + checkpoints[i].transform.position.ToString());
		}
	}


	// Update is called once per frame
	void Update () {
		meemoPosition = meemo.transform.position;
		UpdateLatestCheckPoint ();
		if (meemo.current_state == Hero_Interaction.MeemoState.Dead) {
			//meemo.transform.position
		}
	}


	void UpdateLatestCheckPoint() {
		
	}

}
