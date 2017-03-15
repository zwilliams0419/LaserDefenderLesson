using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed = 7f;
    public int maxHealth = 100;
    public GameObject laser;
    public int damage = 100;
    public float shotSpeed = 10f;
    public float fireRate = .25f;
    public GameObject explosion;        //Takes in a Gameobject with both a particle system and audio source
    public Slider healthBar;
    public float GameOverTime = 5f;
    
    private Sprite playerSprite;
    private AudioSource audio;
    private int currHealth;
    private float xMin, xMax;
    private bool alive = true;

	// Use this for initialization
	void Start () {
        float screenBoundLeft, screenBoundRight;
        float distance = transform.position.z - Camera.main.transform.position.z;

        playerSprite = GetComponent<SpriteRenderer>().sprite;
        screenBoundLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
        screenBoundRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
        xMin = screenBoundLeft + playerSprite.bounds.extents.x;
        xMax = screenBoundRight - playerSprite.bounds.extents.x;

        audio = GetComponent<AudioSource>();

        currHealth = maxHealth;
        healthBar.value = (float)currHealth / maxHealth;
    }
	
	// Update is called once per frame
	void Update () {

        if (alive)
        {
            HandleMovement();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                InvokeRepeating("FireLaser", 0f, 0.25f);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                CancelInvoke();
            }
        }
    }


    void HandleMovement()
    {
        float newXPos = transform.position.x;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newXPos += -speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            newXPos += speed * Time.deltaTime;
        }

        newXPos = Mathf.Clamp(newXPos, xMin, xMax);

        transform.position = new Vector2(newXPos, transform.position.y);
    }


    void FireLaser()
    {
        float shotOffset = playerSprite.bounds.extents.y;   //Makes the projectile spawn at the front of the ship

        GameObject shot = Instantiate(laser, transform.position + new Vector3(0,shotOffset,0), Quaternion.identity) as GameObject;

        shot.GetComponent<Rigidbody2D>().velocity = Vector2.up * shotSpeed;
        shot.GetComponent<Projectile>().damage = damage;

        audio.Play();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        TakeHit(projectile);
    }


    private void TakeHit(Projectile projectile)
    {
        currHealth -= projectile.GetDamage();
        healthBar.value = (float)currHealth / maxHealth;
        projectile.Hit();

        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (alive)
        {
            GetComponent<Animator>().SetTrigger("Die");
            GetComponentInChildren<ParticleSystem>().Stop();
            CancelInvoke();
            alive = false;
            Invoke("EndGame", GameOverTime);
        }
    }


    public void PlayExplosion()
    {
        GameObject exp = Instantiate(explosion, transform.position, Quaternion.identity);
        exp.GetComponent<ParticleSystem>().Play();
        exp.GetComponent<AudioSource>().Play();
    }


    private void EndGame()
    {
        GameObject.FindObjectOfType<LevelManager>().GetComponent<LevelManager>().LoadLevel("GameOver");
    }
}
