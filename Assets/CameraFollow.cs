using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public Rigidbody targetRb;

    public float smoothSpeed = .125f;
    public Vector3 offset;

    private void FixedUpdate() {

        // Move Y (up)
        float zoomOutSpeed = 2.5f;
        float playerSpeed = Mathf.Sqrt(Mathf.Pow(targetRb.velocity.x, 2) + Mathf.Pow(targetRb.velocity.z, 2));
        Vector3 zoomOut = new Vector3(0, playerSpeed/zoomOutSpeed, 0);

        // Move X/Z (around)
        Vector3 goalPosition = target.position + offset + zoomOut;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, goalPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;



    }
}
