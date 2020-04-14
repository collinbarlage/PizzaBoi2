using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class planet : MonoBehaviour {
    public Rigidbody rb;
    public Text eText;

    private string name;
    private bool isPlayerIntersect;

	void Start () {
        rb.AddTorque(0, 3f, 0);

        // set name
        name = "Big Mama IV";
	}

	private void OnTriggerEnter(Collider other) {
        if (other.name == "spaceship3") {
            Debug.Log("~~~~~~~~~~~~~~~~~BIG MAMA");
            eText.text = "Press 'e' to enter planet " + name;
            isPlayerIntersect = true;
            
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.name == "spaceship3") {
            eText.text = "";
            isPlayerIntersect = false;
        }
    }

    private void enterPlanet() {
        Time.timeScale = 0.0f;
    }

    private void exitPlanet() {
        Time.timeScale = 1.0f;
    }

    public void inputE()
    {
        Debug.Log("~~~~~~~~~~~~~~~~~inputE1");
        if (isPlayerIntersect) {
            enterPlanet();
        }
    }

}
