using UnityEngine;
using System.Collections;

public class StarBar_interaction : MonoBehaviour {
	private CameraBehavior main_camera;
	private float width;
	private float MIN_BAR_WIDTH = 0f;
	private const float PERCENT_OF_CAMERA_WIDTH = 0.50f;
	private const float PERCENT_OF_CAMERA_HEIGHT = 0.07f;
	private float bar_ratio = Hero_Interaction.START_STAR_TIMER_RATIO;
	private float max_width_relative_to_cam;
	private float current_bar_ratio = 0f;

	private float transition_start;
	// Use this for initialization
	void Start () {
		UpdateStarBarInCamera ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	// deterimine position and size relative to the camera of the starbar
	public void UpdateStarBarInCamera() {
		float duration = (Time.time - transition_start)/1f;
		current_bar_ratio = Mathf.SmoothStep (current_bar_ratio, bar_ratio, duration);
		Debug.Log (current_bar_ratio);
		float cam_height = Camera.main.orthographicSize;
		float cam_width = cam_height * Camera.main.aspect;
		// get width of bar and height of bar
		this.width = cam_width * PERCENT_OF_CAMERA_WIDTH * this.current_bar_ratio; 
		float height = cam_height * PERCENT_OF_CAMERA_HEIGHT; // height is 5 % of the screen
		// get position of bar
		float x = Camera.main.transform.position.x;
		float y = Camera.main.transform.position.y + cam_height - height;
		max_width_relative_to_cam = cam_width * PERCENT_OF_CAMERA_WIDTH;
		float x_offset = (1f - this.current_bar_ratio) * max_width_relative_to_cam / 2f;
		this.transform.position = new Vector3 (x - x_offset, y, 0f);
		this.transform.localScale = new Vector3 (width, height, 0f);
	}

	public void UpdateStarBarSize(float timer_left) {
		float ratio = timer_left / Hero_Interaction.MAX_STAR_TIMER;
		if (ratio < MIN_BAR_WIDTH)
			return;
		this.bar_ratio = ratio;
		this.transition_start = Time.time;
	}

	public void UpdateStarBarSizeInstant(float timer_left) {
		float ratio = timer_left / Hero_Interaction.MAX_STAR_TIMER;
		if (ratio < MIN_BAR_WIDTH)
			return;
		this.bar_ratio = ratio;
		this.transition_start = 0f;
	}
}
