using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerMovement : MonoBehaviour {

    public Rigidbody rb;
    public Rigidbody pizza;
    public Transform barrel;
    public float     boosterStrength = 10; //Power of booster  
    public float     blasterStrength = 925000000000; //Power of blaster  

    public GameObject astro;
    public GameObject mob;
    public GameObject planet;
    public GameObject bigPizza;

    float chunkSize = 20f;
    float spawnThickness  = 1f;
    Chunk currentChunk;
    List<Chunk> chunks    = new List<Chunk>();
    List<Chunk> adjacents = new List<Chunk>();


    private bool isBoost = false;
    private bool isLeft = false;
    private bool isRight = false;
    private bool isBack = false;
    private bool isTractor = false;
    private bool isPowerDown = false;

    public Image fuelBar;
    private float fuel = 1f;

    public Text pizzaCount;
    public Text shroomCount;
    private int numPizzas = 25;
    private int numShroom = 0;

    public AudioClip soundBoost; private AudioSource audioBoost;
    public AudioClip soundTractor;   private AudioSource audioTractor;
    public AudioClip soundPowerDown; private AudioSource audioPowerDown;
    public AudioClip soundGet;       private AudioSource audioGet;

    public AudioClip soundA; private AudioSource audioA;
    public AudioClip soundB; private AudioSource audioB;
    public AudioClip soundC; private AudioSource audioC;

    public AudioClip soundHuayh; private AudioSource audioHuayh;

    // Use this for initialization
    void Start () {
        Debug.Log("ayyyeo it's pizza boi"); // awe yeee 
        rb.useGravity = false; // outerspace

        // Spawn inital chunk
        currentChunk = new Chunk(0, 0);
        Chunk init = new Chunk(0, 0);
        chunks.Add(init);

        // Setup adjacents
        adjacents.Add(new Chunk(0, 1));  adjacents.Add(new Chunk(1,1));
        adjacents.Add(new Chunk(1,0));   adjacents.Add(new Chunk(1,-1));
        adjacents.Add(new Chunk(0, -1)); adjacents.Add(new Chunk(-1,-1));
        adjacents.Add(new Chunk(-1, 0)); adjacents.Add(new Chunk(-1,1));

        // Set up audio
        audioBoost = AddAudio(soundBoost, 0.3f);
        audioA     = AddAudio(soundA,     0.0f); 
        audioB     = AddAudio(soundB,     0f); 
        audioC     = AddAudio(soundC,     0f); 
        audioHuayh = AddAudio(soundHuayh, 0.4f, false);
        audioTractor = AddAudio(soundTractor, 0.1f, false);
        audioPowerDown = AddAudio(soundPowerDown, 0.5f, false);
        audioGet = AddAudio(soundGet, 0.2f, false);
	}

    // On Collision
    private void OnCollisionEnter(Collision collision) {
        audioHuayh.Play(0);
    }

    // Update is called once per frame
    void Update () {

        // Rotate based on mouse position
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, -1 * angle * Mathf.Rad2Deg - 90f));

        // Check button state
        if (Input.GetKeyDown("w")) toggleBoost(true);
        if (Input.GetKeyUp  ("w")) toggleBoost(false);

        if (Input.GetMouseButtonDown(0) && numPizzas > 0) firePizza(angle);

        // Perform actions
        if (isBoost && fuel > 0) {
            boost(angle);
        } else {
            audioBoost.Stop();
            if (fuel <  0f && !isPowerDown) {
                audioPowerDown.Play();
                isPowerDown = true;
            }
        }
        // Check music switch
        updateMusic(rb.position);

        // Update spawner
        checkSpawnStatus(rb.position);

        // Check for quit
        if (Input.GetKey("escape")) {
            Application.Quit();
        }
    }

    // Boost
    void toggleBoost(bool b) {
        isBoost = b;
        if(isBoost) {
            if(fuel > 0) audioBoost.Play(0);
        } else { audioBoost.Stop(); }
    }


    void boost(float angle) {
        float xForce = Mathf.Cos(angle) * -1 * boosterStrength;
        float zForce = Mathf.Sin(angle) * -1 * boosterStrength;
        rb.AddForce(xForce, 0, zForce);
        fuel -= .003f; // use fuel
        fuelBar.fillAmount = fuel; // update fuel bar
        if(fuel < .3) {
            fuelBar.color = new Color(0.80f, 0.00f, 0.00f);
        } else if(fuel < .5) {
            fuelBar.color = new Color(1.00f, 0.75f, 0.00f);
        }

    }
    void back(float angle) {
        float boosterStrength = 5; //Power of booster 
        float xForce = Mathf.Cos(angle) * boosterStrength;
        float zForce = Mathf.Sin(angle) * boosterStrength;
        rb.AddForce(xForce, 0, zForce);
    }


    // Pizza
    void firePizza(float angle) {
        Rigidbody pizzaInstance;
        float xForce = Mathf.Cos(angle) * -1;
        float zForce = Mathf.Sin(angle) * -1;
        Vector3 barrelEnd = new Vector3(barrel.position.x + xForce*.8f, barrel.position.y, barrel.position.z + zForce*.8f);

        // Spawn Pizza
        numPizzas -= 1;
        pizzaCount.text = numPizzas.ToString();
        pizzaInstance = Instantiate(pizza, barrelEnd, barrel.rotation) as Rigidbody;
        pizzaInstance.AddForce(xForce * blasterStrength + rb.velocity.x, 0, zForce * blasterStrength + rb.velocity.z, ForceMode.VelocityChange);
        // Rebound force
        float reboundForce = -8f;
        rb.AddForce(xForce * reboundForce, 0, zForce * reboundForce);
        // Ignore collision with ship
        Physics.IgnoreCollision(pizzaInstance.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Tractor beam
    private void OnTriggerStay(Collider shroom) {
        if (shroom.name == "shroom1(Clone)") {
            Rigidbody shroomRb = shroom.GetComponent<Rigidbody>();
            
            float tractorBeamStrength = -5;
            float xComponent = shroomRb.position.x - rb.position.x;
            float zComponent = shroomRb.position.z - rb.position.z;
            float tractorBeam = tractorBeamStrength * (1 / Mathf.Sqrt(Mathf.Pow(xComponent,2) + Mathf.Pow(zComponent, 2)));

            shroomRb.AddForce(tractorBeam * xComponent,  0,  tractorBeam * zComponent);

            if (distanceBetweenTwoPoints(shroomRb.position, rb.position) < 1) {
                audioGet.Play();
                // Add shroom to inventory
                numShroom += 1;
                shroomCount.text = numShroom.ToString();
                Destroy(shroom.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.name == "shroom1(Clone)" && !audioTractor.isPlaying) {
            audioTractor.Play();
            isTractor = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.name == "shroom1(Clone)") {
            audioTractor.Stop();
            isTractor = false;
        }
    }

    // Music
    bool A = false, B = false, C = false;
    void updateMusic(Vector3 pos) {
        if (pos.x < 10 && pos.x > -10 && !A) {
            audioA.Play();
            A = true;
        }
    }


    // Helper functions
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x);
    }

    float distanceBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Sqrt(Mathf.Pow(b.x - a.x, 2)+ Mathf.Pow(b.y - a.y, 2) + Mathf.Pow(b.z - a.z, 2));
    }

    public AudioSource AddAudio(AudioClip clip, float vol, bool looping = true) { 
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.playOnAwake = false;
        newAudio.loop = looping;
        newAudio.clip = clip; 
        newAudio.volume = vol; 
        return newAudio; 
    }


    // Spawn space stuff
  
    void checkSpawnStatus(Vector3 pos) {
        // Get chunk coords
        int xChunk = (int)Mathf.Floor(pos.x / chunkSize);
        int zChunk = (int)Mathf.Floor(pos.z / chunkSize);
        // check if user entered new chunk
        if (currentChunk.x != xChunk || currentChunk.z != zChunk) {
            Debug.Log("Ayy - you just entered Chunk: (" + xChunk + ", " + zChunk + ")");
            currentChunk.x = xChunk; currentChunk.z = zChunk;
            spawnEnv(xChunk, zChunk);
        }
    }

    void spawnEnv(int xChunk, int zChunk) {
            Debug.Log("spawnEnv");
        // Check if chunk is unspawned
        for (int i=0; i<8; i++) { // for all adjacent tiles
            Debug.Log("wat");

            if (!findChunk(i, xChunk, zChunk)) { // search for unspawned chunks
                Debug.Log("SPAWNED ADJ CHUNK BOI");
                spawnChunk(adjacents[i].x + xChunk, adjacents[i].z + zChunk);
            }
        }
    }

    bool findChunk(int i, int x, int z) {
        bool foundChunk = false;
        foreach (Chunk c in chunks) { 
            if (adjacents[i].x + x == c.x && adjacents[i].z + z == c.z) {
                foundChunk = true;
                break;
            }
        }
        Debug.Log("foundCunk is "+ foundChunk);
        return foundChunk;
    }

    void spawnChunk(int x, int z) {
        Debug.Log("WHOA - I'm spawning Chunk: (" + x + ", " + z + ")");
        chunks.Add(new Chunk(x, z));

        //Spawn space stuff here!
        float spawnArea = Mathf.Pow(chunkSize, 2);
        int spawnCount = Mathf.RoundToInt(spawnArea * spawnThickness);
        for (int i=0; i<spawnCount; i++) {
            GameObject chunkObj;
            float r = Random.Range(0f, 1);
            float q = Random.Range(0f, 1);
            Vector3 pos = new Vector3((r-.5f)*chunkSize+x * chunkSize, 2.95f, (q - .5f) * chunkSize+z * chunkSize);
            if (r < .7) {       //70%
                //asteriod
                chunkObj = Instantiate(astro, pos, new Quaternion(0,0,0,0)) as GameObject;
            } else if(r < .9) { //20% 
                // pizza
                chunkObj = Instantiate(bigPizza, pos, new Quaternion(0,0,0,0)) as GameObject;
            } else if(r < .99) { //9%
                //mob
                chunkObj = Instantiate(mob, pos, new Quaternion(0,0,0,0)) as GameObject;
            } else {             //1%
                //planet
                chunkObj = Instantiate(planet, pos, new Quaternion(0,0,0,0)) as GameObject;
            }
        }

    }

}

public class Chunk {
     
    public Chunk(int cX, int cZ) {
        x = cX;
        z = cZ;
        chunkBodies = new List<Rigidbody>();
    }

    public List<Rigidbody> chunkBodies;

    public int x { set; get; }
    public int z { set; get; }
}
