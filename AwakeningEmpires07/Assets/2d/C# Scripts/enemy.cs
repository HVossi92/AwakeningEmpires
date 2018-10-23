using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {

    Rigidbody2D rb;
    public GameObject bullet,explosion,hit,battery, ammo;

    public float xSpeed;
    public float ySpeed;

    public int score;

    public bool  canShoot;
    public float fireRate;
    public float health;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }


    void Start () {
 
        if (!canShoot) return;
        fireRate = fireRate + (Random.Range(fireRate / -5, fireRate / 5));
            InvokeRepeating("Shoot", fireRate,fireRate);
    }
	

	void Update () {
        rb.velocity = new Vector2(xSpeed,ySpeed*-1);

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag=="player")
        {
            col.gameObject.GetComponent<player_controls>().Damage();
            Die();
        
        }
    }

    public void Die(){
        if((int)Random.Range(0,3)==0)
            Instantiate(battery, transform.position, Quaternion.identity);
            if((int)Random.Range(0,3)==0)
            Instantiate(ammo, transform.position, Quaternion.identity);
        Instantiate(explosion, transform.position, Quaternion.identity);
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + score);
        Destroy(gameObject);

    }

    public void Damage()
    {
        Instantiate(hit, transform.position, Quaternion.identity);
        health--;
        StartCoroutine(Blink());
        if (health == 0)
            Die();


    }
    IEnumerator Blink()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }

    void Shoot()
        {
            GameObject temp = (GameObject) Instantiate(bullet,transform.position,Quaternion.identity);
            temp.GetComponent<bullet>().ChangeDirection();
        }


}
