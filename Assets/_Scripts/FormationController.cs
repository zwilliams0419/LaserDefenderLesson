using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {

    public GameObject enemyPrefab;  //The enemy to be spawned in the formation
    public float width = 10f;
    public float height = 5f;
    public float speed = 5f;
    public float spawnDelay = .5f;
    public int pointBonus = 200;

    private float xMin, xMax;
    private Vector3 velocity;


    // Use this for initialization
    void Start () {
        float screenBoundLeft, screenBoundRight;
        float distance = transform.position.z - Camera.main.transform.position.z;

        SpawnUntilFull();

        //Determine the horizontal bounds for the formation based on the screen edges
        screenBoundLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
        screenBoundRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
        xMin = screenBoundLeft + (width / 2);
        xMax = screenBoundRight - (width / 2);

        velocity = new Vector3(speed, 0, 0);
    }
	

	// Update is called once per frame
	void Update () {
        
        //Move formation
        transform.position += velocity * Time.deltaTime;

        //Reverse direction if horizontal bounds are exceeded
        if (transform.position.x > xMax)
        {
            //transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), transform.position.y, transform.position.z);
            velocity = speed * Vector3.left;
        }
        else if(transform.position.x < xMin)
        {
            velocity = speed * Vector3.right;
        }

        if (AllMembersDead())
        {
            SpawnUntilFull();
        }
        
	}


    private bool AllMembersDead()
    {
        //Check whether any of the positions have an enemy ship in them
        foreach (Transform childPosition in transform)
        {
            if (childPosition.childCount > 0)
            {
                return false;
            }
        }

        FindObjectOfType<ScoreKeeper>().ChangeScore(pointBonus);
        return true;
    }


    private Transform NextFreePosition()
    {
        foreach (Transform childPosition in transform)
        {
            if (childPosition.childCount == 0)
            {
                return childPosition;
            }
        }

        return null;
    }


    /*private void SpawnEnemies()
    {
        foreach (Transform child in transform)
        {
            Instantiate(enemyPrefab, child.position, Quaternion.identity, child);
        }
    }*/


    private void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();

        if(freePosition)
        {
            Instantiate(enemyPrefab, freePosition.position, Quaternion.identity, freePosition);
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3 (width, height));
    }
}
