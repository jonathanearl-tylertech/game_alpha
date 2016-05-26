using UnityEngine;
using System.Collections;

public class MenuCameraBound : MonoBehaviour {
    public static MenuCameraBound globalBehavior;
    private Camera mMainCamera;
    private Bounds mWorldBound;
    private Vector2 mWorldMin;
    private Vector2 mWorldMax;
    private Vector2 mWorldCenter;
    public GameObject mEnemyToSpawn = null;
    private float mPreEnemySpawnTime = -1f;
    private const float mEnemySpawnInterval = 3.0f; // within 3 seconds
    private GameObject e = null;
    public bool enemyMovment = false;
    public int countEgg = 0;
    public int countEnemy = 50;
    private float ranX;
    private float ranY;


    // Use this for initialization
    void Start () {
        globalBehavior = this;
        mMainCamera = Camera.main;
        mWorldBound = new Bounds(Vector3.zero, Vector3.one);
        updateWorldWindowBound();
        ranX = Random.Range(mWorldMin.x, mWorldMax.x);
        ranY = Random.Range(mWorldMin.y, mWorldMax.y);
        for (int i = 0; i < 5; i++)
        {
            ranX = Random.Range(mWorldMin.x, mWorldMax.x);
            ranY = Random.Range(mWorldMin.y, mWorldMax.y);
            mEnemyToSpawn = Resources.Load("Prefabs/StaticBubblePrefab") as GameObject;
            e = (GameObject)Instantiate(mEnemyToSpawn, new Vector3(ranX, ranY, 0f), Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (enemyMovment)
            spawnAnEnemy();

    }

    public enum WorldBoundStatus
    {
        CollideTop,
        CollideLeft,
        CollideRight,
        CollideBottom,
        Outside,
        Inside
    }

    public void updateWorldWindowBound()
    {
        if(mMainCamera != null)
        {
            float maxY = mMainCamera.orthographicSize;
            float maxX = mMainCamera.orthographicSize * mMainCamera.aspect;
            float sizeX = 2 * maxX;     // Width of the game window
            float sizeY = 2 * maxY;     // Height of the game window
            float sizeZ = Mathf.Abs(mMainCamera.farClipPlane - mMainCamera.nearClipPlane);

            Vector3 c = mMainCamera.transform.position;
            c.z = 0.0f; // Using 2D so make sure z-component is always zero
            mWorldBound.center = c;
            mWorldBound.size = new Vector3(sizeX, sizeY, sizeZ);

            mWorldCenter = new Vector2(c.x, c.y);
            mWorldMin = new Vector2(mWorldBound.min.x, mWorldBound.min.y);
            mWorldMax = new Vector2(mWorldBound.max.x, mWorldBound.max.y);
        }
    }

    public Vector2 WorldCenter { get { return mWorldCenter;  } }
    public Vector2 WorldMin { get { return mWorldMin; } }
    public Vector2 WorldMax { get { return mWorldMax; } }

    public WorldBoundStatus objectCollideWorldBound(Bounds objBound)
    {
        WorldBoundStatus status = WorldBoundStatus.Inside;
        if (mWorldBound.Intersects(objBound))
        {
            if (objBound.max.x > mWorldBound.max.x)
                status = WorldBoundStatus.CollideRight;
            else if (objBound.min.x < mWorldBound.min.x)
                status = WorldBoundStatus.CollideLeft;
            else if (objBound.max.y > mWorldBound.max.y)
                status = WorldBoundStatus.CollideTop;
            else if (objBound.min.y < mWorldBound.min.y)
                status = WorldBoundStatus.CollideBottom;
            else if ((objBound.min.z < mWorldBound.min.z) || (objBound.max.z > mWorldBound.max.z))
                status = WorldBoundStatus.Outside;
        }
        else    status = WorldBoundStatus.Outside;

        return status;
    }

    private void spawnAnEnemy()
    {
        if (((Time.realtimeSinceStartup - mPreEnemySpawnTime) > mEnemySpawnInterval) && enemyMovment) 
        {
            float posX = Random.Range(mWorldMin.x, mWorldMax.x);
            float posY = Random.Range(mWorldMin.y, mWorldMax.y);
            e = (GameObject)Instantiate(mEnemyToSpawn, new Vector3(posX, posY, 0f), Quaternion.identity);
            countEnemy++;
            mPreEnemySpawnTime = Time.realtimeSinceStartup;
        }
    }
}
