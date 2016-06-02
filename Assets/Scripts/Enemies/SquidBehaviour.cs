using UnityEngine;
using System.Collections;

public class SquidBehaviour : MonoBehaviour {
	public float distance = 2f;
	public float speed = 1f;
	public Vector3 initpos;
	private float maxY;
	private float minY;
	private bool isMovingUp;

	// Use this for initialization
	void Start () {
		initpos = transform.position;
		maxY = initpos.y + distance;
		minY = initpos.y - distance;
		isMovingUp = true;
	}
	
	// Update is called once per frame
	void Update () {
		float currentY = transform.position.y; 
		if (currentY > maxY) {
			isMovingUp = false;
		} else if (currentY < minY) {
			isMovingUp = true;
		}
		if (isMovingUp) {
			MoveUp ();
		} else {
			MoveDown ();
		}
	}

	private void MoveUp(){
		transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
	}

	private void MoveDown(){
		transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
	}
}
