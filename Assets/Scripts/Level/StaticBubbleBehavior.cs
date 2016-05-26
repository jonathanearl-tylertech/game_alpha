using UnityEngine;
using System.Collections;

public class StaticBubbleBehavior : MonoBehaviour {
    public float mSpeed = Random.Range(1f, 5f);
    public float mTowardsCenter = 1f;
    //private EnemyState currentState;
    public bool enemyMove = false;

    // Use this for initialization
    void Start () {
        newDirection();
        //currentState = EnemyState.Normal;
	}
	
	// Update is called once per frame
	void Update () {
        MenuCameraBound globalBehavior = GameObject.Find("MenuManager").GetComponent<MenuCameraBound>();
        //if (Input.GetKeyUp(KeyCode.Space))//(Input.GetAxis("Jump") > 0)
        //{
        //    enemyMove = !enemyMove;
        //    globalBehavior.enemyMovment = enemyMove;
        //}

        //if((currentState != EnemyState.Stunned) && enemyMove)
            transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;


        MenuCameraBound.WorldBoundStatus status = globalBehavior.objectCollideWorldBound(GetComponent<Renderer>().bounds);

        if(status != MenuCameraBound.WorldBoundStatus.Inside)
        {
            newDirection();
        }


    }

    private void newDirection()
    {
        MenuCameraBound globalBehavior = GameObject.Find("MenuManager").GetComponent<MenuCameraBound>();

        // we want to move towards the center of the world
        Vector2 v = globalBehavior.WorldCenter - new Vector2(transform.position.x, transform.position.y);
        // this is vector that will take us back to world center
        //v.Normalize();
        Vector2 vn = new Vector2(v.y, -v.x); // this is a direciotn that is perpendicular to V

        float useV = 1.0f - Mathf.Clamp(mTowardsCenter, 0.01f, 1.0f);
        float tanSpread = Mathf.Tan(useV * Mathf.PI / 2.0f);
        float randomX = Random.Range(0f, 1f);
        float yRange = tanSpread * randomX;
        float randomY = Random.Range(-yRange, yRange);

        Vector2 newDir = randomX * v + randomY * vn;
        newDir.Normalize();
        transform.up = newDir;
    }
}
