using UnityEngine;
using System.Collections;
using System;

public class RespawnManager : MonoBehaviour {
	public const int MAX_RESPAWN_TIMES = 3;
	public GameObject[] checkpoints;
	public int latestCheckPointIndex;
	public Hero_Interaction meemo;
	public Vector3 meemoPosition;
	public int respawnTime = 0;
	public Canvas gameOverCanvas;
	public HealthBar_interaction healthBar;


	// Use this for initialization
	void Start () {
		checkpoints = GameObject.FindGameObjectsWithTag ("checkPoint");
		meemo = GameObject.Find ("Meemo").GetComponent<Hero_Interaction> ();
		gameOverCanvas = GameObject.Find ("GameOverCanvas").GetComponent<Canvas> ();
		healthBar = GameObject.Find ("HealthBar").GetComponent<HealthBar_interaction> ();
	}


	// Update is called once per frame
	void Update () {
		meemoPosition = meemo.transform.position;

		if (meemo.current_state != Hero_Interaction.MeemoState.Dead) {
			UpdateLatestCheckPoint ();
		}
		else {
			//respawn meemo
			meemo.transform.position = new Vector3(checkpoints[latestCheckPointIndex].transform.position.x, checkpoints[latestCheckPointIndex].transform.position.y + 2f, 0f);
			//current_state = Normal
			meemo.current_state = Hero_Interaction.MeemoState.Normal;
			//refill HealthBar
			healthBar.curNumOfHearts = 5;
			//update respawn times
			respawnTime++;
			if (respawnTime > MAX_RESPAWN_TIMES) {
				meemo.Die ();
				gameOverCanvas.enabled = true;
			}
		}
	}


	void UpdateLatestCheckPoint() {
		meemoPosition = meemo.transform.position;
		Vector3 checkpointPos;
		float minDist = 1000f;

		for (int i = 0; i < checkpoints.Length; i++) {
			checkpointPos = checkpoints [i].transform.position;
			if (Vector3.Distance (checkpointPos, meemoPosition) < minDist) {
				minDist = Vector3.Distance (checkpointPos, meemoPosition);
				if (checkpointPos.x < meemoPosition.x) {
					latestCheckPointIndex = i;
				}
			}
		}
	}
}
