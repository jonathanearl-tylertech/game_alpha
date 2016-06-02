using UnityEngine;
using System.Collections;

public class TutorialBubbleBehavior : MonoBehaviour {
	public static float sinAmp = 0.75f;
	public static float sinOsc = 45f;
	public static float bubbleSpeed = 3f;
	private Vector3 bgSize;

	public float distance = 1f;
	public float speed = 1f;
	public Vector3 initpos;
	private float maxY;
	private float minY;
	private bool isMovingUp;

	private TBState State;
	private enum TBState
	{
		Station,
		Floating
	}
	// Use this for initialization
	void Start () {
		bgSize = GameObject.Find ("backgroundImage").GetComponent<Renderer> ().bounds.size;
		State = TBState.Station;
		initpos = transform.position;
		maxY = initpos.y + distance;
		minY = initpos.y - distance;
		isMovingUp = true;
	}
	
	// Update is called once per frame
	void Update () {
		switch (State) {
		case TBState.Station:
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
			break;
		case TBState.Floating:
			FollowSineCurve ();
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			State = TBState.Floating;
		}
	}

	private void MoveUp(){
		transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
	}

	private void MoveDown(){
		transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
	}

	public void FollowSineCurve(){
		float newY = transform.position.y + bubbleSpeed * Time.deltaTime;
		float newX = initpos.x + GetXValue (newY); 
		transform.position = new Vector3 (newX, newY, 0f);
	}

	private float GetXValue(float y){
		float sinFreqScale = sinOsc * 2f * (Mathf.PI) / bgSize.y;
		return sinAmp * (Mathf.Sin(y * sinFreqScale));
	}
}
