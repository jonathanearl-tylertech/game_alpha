using UnityEngine;
using System.Collections;

public class Angler_interaction : MonoBehaviour {

	private float start_x;
	public float max_distance_to_travel = 10f;
	public float current_distance_traveled = 0f;
	public float speed = 2f;
	private float travel_direction = 1f;
	// Use this for initialization
	void Start () {
		this.start_x = this.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		current_distance_traveled += speed * Time.deltaTime;
		if (current_distance_traveled > max_distance_to_travel) {
			travel_direction *= -1f;
			current_distance_traveled = 0f;
		} 
		float offset = travel_direction * speed * Time.deltaTime;
		this.transform.position = 
			new Vector3 (transform.position.x + offset, transform.position.y, transform.position.z); 

	}
}
