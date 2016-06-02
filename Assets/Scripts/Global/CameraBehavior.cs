using UnityEngine;
using System.Collections;

public class CameraBehavior: MonoBehaviour {

	//private float BUFFER;// = 10f;
    #region World Bound support
    private Bounds mWorldBound;  // this is the world bound
    private Vector2 mWorldMin;  // Better support 2D interactions
    private Vector2 mWorldMax;
    private Vector2 mWorldCenter;
    #endregion


	public float globalyMax;
	public float globalxMax;
	public float globalyMin;
	public float globalxMin;

	public float cameraMinx;
	public float cameraMaxx;
	public float cameraMiny;
	public float cameraMaxy;

	private Transform target;
	private Transform cam;

    //public GameObject mZoomRef = null;
    public Camera mCamera = null;

	#region UI support
	private StarBar_interaction star_bar = null;
	private HealthBar_interaction health_bar = null;
	#endregion

    // Use this for initialization
    void Start () {


        mCamera = GetComponent<Camera>();
		target = GameObject.Find("Meemo").transform;
		cam = GameObject.Find ("Main Camera").transform;

        // World bound support
        mWorldBound = new Bounds(Vector3.zero, Vector3.one);
        UpdateWorldWindowBound();

		cameraMinx = globalxMin + mCamera.orthographicSize * mCamera.aspect;
		cameraMaxx = globalxMax - mCamera.orthographicSize * mCamera.aspect;
		cameraMiny = globalyMin + mCamera.orthographicSize;
		cameraMaxy = globalyMax - mCamera.orthographicSize;

		this.star_bar = GameObject.Find ("StarBar").GetComponent<StarBar_interaction> ();
		this.health_bar = GameObject.Find ("HealthBar").GetComponent<HealthBar_interaction> ();
    }

	void LateUpdate(){
		if(GameObject.Find("Meemo").GetComponent<Hero_Interaction>().current_state != Hero_Interaction.MeemoState.Dead)
			transform.position = new Vector3 (Mathf.Clamp (target.position.x, cameraMinx, cameraMaxx), Mathf.Clamp (target.position.y, cameraMiny, cameraMaxy), transform.position.z);  

		// limits the hero from moving backwards
		//cameraMinx = cam.position.x;

		this.star_bar.UpdateStarBarInCamera ();
		this.health_bar.UpdatePosition ();
	}

	void Update() {
	}

    #region Game Window World size bound support
    public enum WorldBoundStatus
    {
        CollideTop,
        CollideLeft,
        CollideRight,
        CollideBottom,
        Outside,
        Inside
    };

    /// <summary>
    /// This function must be called anytime the MainCamera is moved, or changed in size
    /// </summary>
	/// 
    public void UpdateWorldWindowBound()
    {
		Vector3 backgroundSize = GameObject.Find ("backgroundImage").GetComponent<Renderer> ().bounds.size;
		Debug.Log (backgroundSize);
		Vector3 backgroundPos = GameObject.Find ("backgroundImage").GetComponent<Renderer> ().transform.position;

        float maxY = mCamera.orthographicSize;
        float maxX = mCamera.orthographicSize * mCamera.aspect;
        float sizeX = 2 * maxX;
        float sizeY = 2 * maxY;
        float sizeZ = Mathf.Abs(mCamera.farClipPlane - mCamera.nearClipPlane);

        // Looking down the positive z-axis
        Vector3 c = mCamera.transform.position;
        c.z += 0.5f * sizeZ;
        mWorldBound.center = c;
        mWorldBound.size = new Vector3(sizeX, sizeY, sizeZ);

        mWorldCenter = new Vector2(c.x, c.y);
        mWorldMin = new Vector2(mWorldBound.min.x, mWorldBound.min.y);
        mWorldMax = new Vector2(mWorldBound.max.x, mWorldBound.max.y);

		//background displacement
		float YDISP = backgroundPos.y;
		float XDISP = backgroundPos.x;

		//initialize global bounds including buffers
		globalxMin = XDISP - backgroundSize.x/2; //-mCamera.orthographicSize * mCamera.aspect + XDISP;
		globalxMax = backgroundSize.x/2 + XDISP; //- (mCamera.orthographicSize * mCamera.aspect)
		globalyMax = backgroundSize.y / 2f + YDISP;
		globalyMin = -backgroundSize.y / 2f + YDISP;
    }

    public Vector2 WorldCenter { get { return mWorldCenter; } }
    public Vector2 WorldMin { get { return mWorldMin; } }
    public Vector2 WorldMax { get { return mWorldMax; } }

    public WorldBoundStatus ObjectCollideWorldBound(Bounds objBound)
    {
        WorldBoundStatus status = WorldBoundStatus.Inside;

        if (mWorldBound.Intersects(objBound))
        {
			if (objBound.max.x > globalxMax) //mWorldBound.max.x)
                status = WorldBoundStatus.CollideRight;
			else if (objBound.min.x < globalxMin) //mWorldBound.min.x)
                status = WorldBoundStatus.CollideLeft;
			else if (objBound.max.y > globalyMax) //mWorldBound.max.y)
                status = WorldBoundStatus.CollideTop;
			else if (objBound.min.y < globalyMin + 1f) //mWorldBound.min.y)
                status = WorldBoundStatus.CollideBottom;
            else if ((objBound.min.z < mWorldBound.min.z) || (objBound.max.z > mWorldBound.max.z))
                status = WorldBoundStatus.Outside;
        }
        else {
            status = WorldBoundStatus.Outside;
        }
        return status;
    }
    #endregion
}
