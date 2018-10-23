using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour {

    public float rate;
    public GameObject[] enemies;
    public int waves = 1;

	void Start () {
        InvokeRepeating("SpawnEnemy",rate,rate);
		
	}
	

	void SpawnEnemy () {
        for(int i=0; i<waves;i++)
        Instantiate(enemies[(int)Random.Range(0,enemies.Length)],new Vector3(Random.Range(-8.5f,8.5f),6,0),Quaternion.identity);
	}
}
