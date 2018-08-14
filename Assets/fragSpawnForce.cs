using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fragSpawnForce : MonoBehaviour {

    public Rigidbody rb;

    void Start()
    {
    	float f = 25f;
        rb.AddForce(Random.Range(-1 * f, f), 0, Random.Range(-1 * f, f));
        rb.AddTorque(Random.Range(-1 * f, f), Random.Range(-1 * f, f), Random.Range(-1 * f, f));
    }
}
