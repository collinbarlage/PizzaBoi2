using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shroom : MonoBehaviour {

    public Collider shroomCol;
    public Rigidbody rb;
    private GameObject ship;

	void Start () {
        float f = 5f;
        rb.AddTorque(Random.Range(-1 * f, f), Random.Range(-1 * f, f), Random.Range(-1 * f, f));

        // Ignore collision with ship
        ship = GameObject.Find("spaceship3");
        Physics.IgnoreCollision(ship.GetComponent<SphereCollider>(), shroomCol);
    }


}
