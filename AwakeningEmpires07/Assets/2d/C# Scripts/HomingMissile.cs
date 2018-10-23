using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour {

	public Transform target;
	public float speed = 5f; 
	public float rotateSpeed = 200f;
	public GameObject explosion;
	private Rigidbody2D rb;


	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("enemy").transform;
		rb = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Vector2 direction = (Vector2)target.position - rb.position;

		direction.Normalize();
		
		float rotateAmount = Vector3.Cross(direction, transform.up).z;

		rb.angularVelocity = -rotateAmount * rotateSpeed;

		rb.velocity = transform.up * speed;
	}

void OnTriggerEnter2D(Collider2D col)

			{
            if (col.gameObject.tag == "enemy")
			Instantiate(explosion,transform.position, transform.rotation);
            {
                col.gameObject.GetComponent<enemy>().Die();
                Destroy(gameObject);
            }
			}
       
}
