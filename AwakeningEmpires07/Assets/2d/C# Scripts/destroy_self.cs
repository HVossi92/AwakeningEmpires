using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_self : MonoBehaviour {

    public float time;
    

	void Start () {
        Destroy(gameObject, time);
		
	}
}
