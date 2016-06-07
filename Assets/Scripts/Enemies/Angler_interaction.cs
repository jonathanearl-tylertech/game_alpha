﻿using UnityEngine;
using System.Collections;

public class Angler_interaction : MonoBehaviour {

	public float max_distance_to_travel = 25f;
	public float current_distance_traveled = 0f;
	public float speed;
	public float travel_direction = 1f;
	public float distFromMeemoToActivateTrigger = 5f;
	public Vector3 initPos;
	public bool isMovingRight = true;

	#region state support
	public enum AnglarState
	{
		Stationary,
		Moving
	}
	public AnglarState currentState;
	#endregion


	// Use this for initialization
	void Start () {
		speed = 0f;
		currentState = AnglarState.Stationary;
	}


	// Update is called once per frame
	void Update () {
		Canvas gameOverCanvas = GameObject.Find ("GameOverCanvas").GetComponent<Canvas> ();

		if (currentState == AnglarState.Stationary && !gameOverCanvas.enabled) {
			Hero_Interaction meemo = GameObject.FindGameObjectWithTag ("Player").GetComponent<Hero_Interaction> ();
			if (meemo.transform.position.x > transform.position.x - 2.5f ){ //Vector3.Distance (meemo.transform.position, transform.position) < distFromMeemoToActivateTrigger) {
				currentState = AnglarState.Moving;
				transform.localScale = new Vector3 (transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
			}
		} else {
			speed = 2.5f;
			if (isMovingRight) {
				current_distance_traveled += speed * Time.deltaTime;
				if (current_distance_traveled < max_distance_to_travel) {
					float offset = travel_direction * speed * Time.deltaTime;
					this.transform.position = 
					new Vector3 (transform.position.x + offset, transform.position.y, transform.position.z);
				} else {
					isMovingRight = false;
					current_distance_traveled = 0f;
					transform.localScale = new Vector3 (transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
				}
			} else {
				current_distance_traveled += speed * Time.deltaTime;
				if (current_distance_traveled < max_distance_to_travel) {
					float offset = travel_direction * speed * Time.deltaTime;
					this.transform.position = 
						new Vector3 (transform.position.x - offset, transform.position.y, transform.position.z);
				} else {
					isMovingRight = true;
					current_distance_traveled = 0f;
					transform.localScale = new Vector3 (transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
				}
			}
		}
	}
}
