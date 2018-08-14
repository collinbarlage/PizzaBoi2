using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteriod : MonoBehaviour {

    public Rigidbody rb;
    public GameObject frags;
    public GameObject shroom;
    public GameObject ship;
    public int numLoot = 5;

    public AudioClip soundBoom; private AudioSource audioBoom;

    // Use this for initialization
    void Start () {
        //rb.rotation = new Quaternion(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f),1);
        rb.AddForce(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
        rb.AddTorque(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));

        audioBoom = AddAudio(soundBoom, 0.9f, false);
    }

    // On Collision
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.name == "pizza2(Clone)") {
            Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            GameObject fragsInstance;


            // save asteroid movment
            Vector3 vel = rb.velocity;
            Vector3 pos = rb.position;
            Quaternion rot = rb.rotation;
            
            Destroy(gameObject);
            fragsInstance = Instantiate(frags, pos, rot) as GameObject;

            //spawn shrooms
            GameObject shroomInstance;
            int sCount = Random.Range(1, numLoot); 
            for(int i=0; i< sCount; i++) {
                // shroom posision
                Vector3 shroomPos = pos;
                shroomPos.y = 2.619f;
                float fs  = 0.4f;
                Vector3 offset = new Vector3(Random.Range(-1*fs, fs), 0, Random.Range(-1*fs, fs));
                shroomInstance = Instantiate(shroom, shroomPos + offset, rot) as GameObject;

                //apply velocites
                shroomInstance.GetComponent<Rigidbody>().velocity = vel;
                Rigidbody[] fragRbs = fragsInstance.GetComponentsInChildren<Rigidbody>();
                for (int r = 0; r < fragRbs.Length; r++) {
                    fragRbs[i].velocity = vel;
                }
            }

            // play sound
            audioBoom.Play();
        }
    }


    public AudioSource AddAudio(AudioClip clip, float vol, bool looping = true)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.playOnAwake = false;
        newAudio.loop = looping;
        newAudio.clip = clip;
        newAudio.volume = vol;
        return newAudio;
    }

}
