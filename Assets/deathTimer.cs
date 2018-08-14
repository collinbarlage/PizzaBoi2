using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathTimer : MonoBehaviour {
	public float deathTime;
    private float startTime;

    void Start () {
        // audioBip = AddAudio(soundBip, 0.4f, false);
        Destroy(gameObject, deathTime);
        // startTime = Time.time;
	}
}
