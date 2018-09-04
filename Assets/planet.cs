using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class planet : MonoBehaviour {
    public Rigidbody rb;
    public Text eText;

    private string name;

	void Start () {
        rb.AddTorque(0, 3f, 0);

        // set name
        name = "Diga IV";
	}

	private void OnTriggerEnter(Collider other) {
        if (other.name == "spaceship3") {
        	Debug.Log("~~~~~~~~~~~~~~~~~BIG MAMA");
            eText.text = "Press 'e' to enter planet " + name;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.name == "spaceship3") {
            eText.text = "";
        }
    }

}
