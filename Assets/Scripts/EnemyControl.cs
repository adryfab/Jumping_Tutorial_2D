using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {

    public float velocity = 2f;

    private Rigidbody2D rg2d;

	// Use this for initialization
	void Start () {
        rg2d = GetComponent<Rigidbody2D>();
        rg2d.velocity = Vector2.left * velocity;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
