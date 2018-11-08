using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controls : MonoBehaviour
{


    Animator anim;
    int delay = 0;
    GameObject a, b, mb, mc;
    public GameObject bullet, hit, explosion, missile;
    Rigidbody2D rb;
    public float speed;
    int health = 3;
    int ammo = 5;
    int rollleftHash = Animator.StringToHash("RollLeft");
    int rollrighttHash = Animator.StringToHash("RollRight");






    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        a = transform.Find("a").gameObject;
        b = transform.Find("b").gameObject;

        mb = transform.Find("mb").gameObject;
        mc = transform.Find("mc").gameObject;
 

    }

    void Start()
    {
        PlayerPrefs.SetInt("Health", health);
        PlayerPrefs.SetInt("Ammo", ammo);
        Die();
        anim = GetComponent<Animator>();

    }

    void Update()
    {

        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed, 0));
        rb.AddForce(new Vector2(0, Input.GetAxis("Vertical") * speed));



        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", move);

        if (Input.GetButton("Fire1") && delay > 10)
            Shoot();

        delay++;

        if (Input.GetButton("Fire3") && delay > 50)
            Missile();
         
        delay++;

        if (Input.GetButtonDown("Roll Left"))


        {
            anim.SetTrigger(rollleftHash);
            delay++;
        }

        if (Input.GetButtonDown("Roll Right"))

        {
            anim.SetTrigger(rollrighttHash);
            delay++;
        }
    }

    public void Damage()
    {

        Instantiate(hit, transform.position, Quaternion.identity);
        health--;
        PlayerPrefs.SetInt("Health", health);
        StartCoroutine(Blink());
        if (health == 0)
            Die();


    }
    IEnumerator Blink()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 1, 1);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }

    void Shoot()
    {
        delay = 0;
        Instantiate(bullet, a.transform.position, Quaternion.identity);
        Instantiate(bullet, b.transform.position, Quaternion.identity);

    }

      public void Missile()
    {
        delay = 0;
  
        Instantiate(missile, mb.transform.position, Quaternion.identity);
        Instantiate(missile, mc.transform.position, Quaternion.identity);
          ammo --;
            PlayerPrefs.SetInt("Ammo", ammo);
            if (ammo == 0)
          Die();
            




    }


    void Die()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void AddHealth()
    {
        health++;
        PlayerPrefs.SetInt("Health", health);
    }
    
     public void AddAmmo()
    {
        ammo++;
        PlayerPrefs.SetInt("Ammo", ammo);
  

    }

}
