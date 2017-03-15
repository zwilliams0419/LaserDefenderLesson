using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int health = 100;
    public GameObject laser;
    public int damage = 10;
    public float shotSpeed = 10f;
    public float averageFireRate = 1f;  //Average fire rate in shots per second (appoximate, doesn't account for minShotTime)
    public float minShotTime = .25f;    //Minimum amount of time between shots
    public int pointValue = 100;
    public GameObject explosion;        //Takes in a Gameobject with both a particle system and audio source

    private AudioSource audio;
    private float minShotTimer;

	// Use this for initialization
	void Start () {
        minShotTimer = minShotTime;
        audio = GetComponent<AudioSource>();
	}
	

	// Update is called once per frame
	void Update () {
        minShotTimer -= Time.deltaTime;

        if ((averageFireRate * Time.deltaTime) > Random.Range(0f, 1f) && minShotTimer <= 0)
        {
            FireShot();
            minShotTimer = minShotTime;
        }
	}


    private void FireShot()
    {
        GameObject shot = Instantiate(laser, transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;

        shot.GetComponent<Rigidbody2D>().velocity = Vector2.down * shotSpeed;
        shot.GetComponent<Projectile>().damage = damage;

        audio.Play();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        TakeHit(projectile);
    }

    private void TakeHit(Projectile projectile)
    {
        health -= projectile.GetDamage();
        projectile.Hit();

        if (health <= 0)
        {
            GameObject exp = Instantiate(explosion, transform.position, Quaternion.identity);
            exp.GetComponent<ParticleSystem>().Play();
            exp.GetComponent<AudioSource>().Play();
            FindObjectOfType<ScoreKeeper>().ChangeScore(pointValue);
            Destroy(gameObject);
        }
    }
}
