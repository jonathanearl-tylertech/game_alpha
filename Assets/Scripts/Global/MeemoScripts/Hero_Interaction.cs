using UnityEngine;
using System.Collections;

public class Hero_Interaction : MonoBehaviour {
	private Canvas gameOverCanvas;
	CameraBehavior globalBehavior;

	public float air_speed = 0.1f;
	private Rigidbody2D rigid_body;
	public Vector3 mSize;

	#region healthbar support
	public static int MAX_HEALTH = 5;
	public int health = MAX_HEALTH;
	HealthBar_interaction health_bar;
	#endregion

	#region movespeed support
	private float max_speed = 7f;
	private float move_speed = 0f;
	private float move_timer;
	private float duration = 0.7f;
	#endregion

	#region jump support
	bool grounded = false;
	public Transform ground_check;
	float ground_radius = 0.3f;
	public LayerMask what_is_ground;
	#endregion

	#region bubble support
	public BubbleBehaviour bubble;
	public bool isInBubble;
	private bool isFacingRight;
	#endregion

	#region starpower support
	public static float MAX_STAR_TIMER = 4f; // get 1 second of power up
	public static float START_STAR_TIMER_RATIO = 0f/4f;
	private const float STAR_POWER_LEVEL = 45f;
	private float star_timer = 0f; 
	private StarBar_interaction star_bar = null;
	private bool is_using_power = false;
	private ParticleSystem PowerAnimation;
	private ParticleSystem damage_particle;
	private Vector3 damage_point;
	#endregion

	#region hurt_state support
	public MeemoState current_state;
	private float hurt_timer = 0f;
	private const float MAX_HURT_TIME = 0.5f;
	private bool is_dimmer = true;
	#endregion

	#region meemostate support
	public enum MeemoState
	{
		Normal,
		Bubble,
		Hurt,
		Invincible,
		Dead
	}
	#endregion

	#region sound support
	public AudioSource[] sounds;
	#endregion

	// Use this for initialization
	void Start () {
		this.damage_point = Vector3.zero;
		this.globalBehavior = Camera.main.GetComponent<CameraBehavior>();
		mSize = GetComponent<Renderer> ().bounds.size;
		this.health_bar = GameObject.Find ("HealthBar").GetComponent<HealthBar_interaction> ();
		this.rigid_body = this.GetComponent<Rigidbody2D>();
		isInBubble = false;
		isFacingRight = true;
		this.star_bar = GameObject.Find ("StarBar").GetComponent<StarBar_interaction> ();
		gameOverCanvas = GameObject.Find ("GameOverCanvas").GetComponent<Canvas> ();
		this.PowerAnimation = GameObject.Find("PowerParticle").GetComponent<ParticleSystem> ();
		var em = this.PowerAnimation.emission; // kinda hacky
		em.enabled = false;
		gameOverCanvas.enabled = false;		// The GameOverCanvas has to be initially enabled on the Unity UI
		current_state = MeemoState.Normal;
		#region movespeed support
		this.move_timer = Time.deltaTime;
		#endregion
		damage_particle = GameObject.Find ("PainParticle").GetComponent<ParticleSystem> ();
		#region sound support
		sounds = GetComponentsInChildren <AudioSource>();
		#endregion

	}

	void FixedUpdate () {
		/// Interaction with bubble
		/// 
		if (Mathf.Abs(this.move_speed) > 0.01f) {
			//			this.rigid_body.AddForce (new Vector2 (move_speed, 0f), ForceMode2D.Force);
			float t = (Time.time - this.move_timer) / this.duration;
			float x_speed = Mathf.SmoothStep( this.rigid_body.velocity.x, this.move_speed, t);
			this.rigid_body.velocity = new Vector3(x_speed, rigid_body.velocity.y, 0f);
		}
		if (is_using_power) {
			fly ();
		}
		this.ClampToCamera ();
		this.CheckDeath ();
		/// End interaction with bubble
	}

	void Update() {
		// turn off powerup
		is_using_power = false;
		var em = this.PowerAnimation.emission;
		em.enabled = false;

		this.grounded = Physics2D.OverlapCircle (this.ground_check.position, this.ground_radius, this.what_is_ground);
		this.move_speed = 0f;

		switch (this.current_state) {
		case MeemoState.Bubble:
			if (Input.GetAxis ("Horizontal") != 0f) { // When meemo is controlling the horizontal direction
				this.move_in_bubble ();
			} else { // When meemo is following bubble
				this.follow_in_bubble ();
			}
			this.change_direction ();
			break;
		case MeemoState.Normal:
			this.service_star_power_update ();
			this.move_speed = Input.GetAxis ("Horizontal") * max_speed;
			this.change_direction ();
			break;
		case MeemoState.Hurt:
			damage_point = this.transform.position;
			damage_particle.transform.position = damage_point;
			damage_particle.Emit(30);
			if (this.health_bar.curNumOfHearts > 0) {
				this.health_bar.curNumOfHearts--;
				sounds [2].Play ();
			}
			if (this.health_bar.curNumOfHearts == 0)
				this.Die ();
			else
				this.current_state = MeemoState.Invincible;
			break;
		case MeemoState.Invincible:
			Renderer renderer = gameObject.GetComponent<Renderer> ();
			Color color = renderer.material.color;
			if (color.a >= 1f) {
				is_dimmer = true;
			} else if (color.a < 0.3f) {
				is_dimmer = false;
			}
			if (is_dimmer)
				color.a -= Time.deltaTime * 10f;
			else
				color.a += Time.deltaTime * 10f;

			renderer.material.color = color;
			this.hurt_timer += Time.deltaTime;
			if (this.hurt_timer > MAX_HURT_TIME) {
				this.current_state = MeemoState.Normal;
				this.hurt_timer = 0f;
				color.a = 1f;
				renderer.material.color = color;
			}
			break;
		}
		// make sure damage particles stay in correct position
		damage_particle.transform.position = damage_point;
	}

	// Currently unused
	//    void Jump ()
	//    {
	//        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
	//    }
	#region starpower support
	void fly () {
		sounds[3].Play ();
		this.star_timer -= Time.fixedDeltaTime;
		this.rigid_body.AddForce (new Vector2 (0f, STAR_POWER_LEVEL), ForceMode2D.Force);
		star_bar.UpdateStarBarSizeInstant (this.star_timer);
	}

	public void ResetStarPower() {
		this.star_timer = Mathf.Min(this.star_timer + MAX_STAR_TIMER/2, MAX_STAR_TIMER);
		star_bar.UpdateStarBarSize (this.star_timer);
	}

	void service_star_power_update() {
		if (Input.GetKey ("space") && this.star_timer > 0f) {
			is_using_power = true;
			var em = this.PowerAnimation.emission;
			em.enabled = true;
		}
	}
	#endregion

	#region direction support
	private void change_direction() {
		bool prev_dir = isFacingRight;
		if (Input.GetAxis ("Horizontal") < 0f && isFacingRight) {
			transform.localScale = new Vector3 (-.3f, .3f, 1f);
			isFacingRight = false;
		}

		if (Input.GetAxis ("Horizontal") > 0f && !isFacingRight) {
			transform.localScale = new Vector3 (.3f, .3f, 1f);
			isFacingRight = true;
		}
		if (prev_dir != isFacingRight) 		
			this.move_timer = Time.deltaTime;
		damage_particle.transform.position = damage_point;
	}
	#endregion

	#region bubble support		
	// Calculate the x value for bubble movement
	private float GetXValue(float y){
		float sinFreqScale = BubbleBehaviour.sinOsc * 2f * (Mathf.PI) / globalBehavior.globalxMax;
		return BubbleBehaviour.sinAmp * (Mathf.Sin(y * sinFreqScale));
	}

	private void move_in_bubble() {
		float bnewY = bubble.transform.position.y + BubbleBehaviour.bubbleSpeed * Time.deltaTime;// bubble floats
		float bnewX = transform.position.x + Input.GetAxis ("Horizontal") * BubbleBehaviour.bubbleSpeed * Time.deltaTime;
		bubble.transform.position = new Vector3 (bnewX, bnewY, 0f);
		bubble.initpos.x = bnewX;

		transform.position = new Vector3 (bnewX, bnewY - 0.2f, transform.position.z);
	}

	private void FollowSineCurve(){
		float newY = bubble.transform.position.y + BubbleBehaviour.bubbleSpeed * Time.deltaTime;
		float newX = bubble.initpos.x + GetXValue (newY); 
		bubble.transform.position = new Vector3 (newX, newY, 0f);
	}

	private void follow_in_bubble() {
		FollowSineCurve();
		// update meemo's position to bubble'e sine curve
		transform.position = new Vector3 (bubble.transform.position.x - 0.05f,
			bubble.transform.position.y - GetComponent<Renderer> ().bounds.size.y / 2f + 0.5f, bubble.transform.position.z);
	}
	#endregion

	#region camera support
	private void ClampToCamera() {
		// Handle when hero collided with the bottom bound of the window (die)
		Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
		Vector3 backgroundSize = GameObject.Find ("backgroundImage").GetComponent<Renderer> ().bounds.size;
		pos.x = Mathf.Clamp (pos.x, 0.03f, 1f - (mSize.x / backgroundSize.x)); //(1f / backgroundSize.x * mSize.x / 2f));
		pos.y = Mathf.Clamp (pos.y, 0.035f, 1f - (mSize.y / backgroundSize.y));
		transform.position = Camera.main.ViewportToWorldPoint (pos);

		CheckDeath ();
	}

	private void CheckDeath() {
		if (transform.position.y - mSize.y/2f <= globalBehavior.globalyMin)
		{
			// Destroy Meemo
			// TimeScale = 0;
			// Panel is active
			Die();
		}
	}

	public void Die() {
		this.current_state = MeemoState.Dead;
		this.transform.position = new Vector3 (-100f, -100f, -100f);
		gameOverCanvas.enabled = true;
	}
	#endregion


	#region sound and power-up support
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "heart")
		{
			HealthBar_interaction healthBar = GameObject.FindGameObjectWithTag ("HealthBar").GetComponent<HealthBar_interaction> ();
			CollectableHeartBehavior heart = other.GetComponent<CollectableHeartBehavior> ();
			if (healthBar.curNumOfHearts < Hero_Interaction.MAX_HEALTH) {
				healthBar.curNumOfHearts++;
			}
			sounds [0].Play ();
			Debug.Log("Meemo touches heart");
			Destroy (heart.gameObject);
		}

		if (other.gameObject.tag == "star")
		{
			CollectableStarBehavior star = other.GetComponent<CollectableStarBehavior> ();
			sounds [1].Play ();
			Debug.Log("Meemo touches star");
			ResetStarPower ();
			Destroy (star.gameObject);
		}
	}
	#endregion

}