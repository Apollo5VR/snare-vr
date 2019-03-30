using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class OculusSpells: MonoBehaviour {
    //public SteamVR_TrackedObject trackedObj;
    //public SteamVR_Controller.Device device;

    //Leviosa & Accio
    private LineRenderer laser;
    //public GameObject teleportAimerObject;
    public LayerMask laserMask; //applies to objects so that only they are interactable (layermasks)
    public bool rightHand;
    public bool leftHand;
    //private OVRInput.Controller thisController;
    public GameObject wandHand;
    public GameObject oculusPlayer;
    public GameObject wandSparks;
    public GameObject wandFlame;
    public GameObject hitObject;
    public float wandGrabPosition; //places object 1 in front of the object 
    public float moveSpeed = 0.001f; //how fast the accio'd object moves to the players wand
    private Ray ray;
    public float spellTime;
    public float spellObjectReset;
    public float spellTimeEnd;
    public bool spellTimerTicking;
    bool spellObjectResetTimer;
    public HPSpeechRecognitionEngine hpSpeechRecognitionEngine;
    public string spellSaid;
    public bool weGotHere;
    public List<GameObject> stupifyProjectiles;
    public GameObject[] haloObjects;
    private Component halo; 
    public int haloCount;
    private GameObject haloObject;
    public GameObject stupifyProjectile;
    public int projectilesAmount;
    public int currentProjectileNumber;
    private Rigidbody projectileRigidbody;
    public GameObject currentProjectile;
    public float projectileForce;
    private bool noShot;
    public ResponseCollector responseCollector;
    public int nextLevel;
    public bool playSound = false;
    AudioSource stupifyAudio;
    AudioSource flameAudio;
    AudioSource AccioAudio;
    bool acciotrue = false;
    public DestroyProjectile destroyProjectile;
    private Vector3 hitObjectInitialPosition;

    //to collect Text script for printing "object collected"
    public TypeWriterEffect typeWriterEffect;


    //private string controllerInputType; //attempting to declare the input type change dynamically, to reduce code duplication
    //WAND - whichever hand grabs the wand, sets rightHand or leftHand;
    //SO - need Grabbable Script, if object is wand, detect if RTouch or LTouch, set rightHand or leftHand


    // Use this for initialization
    void Start() {
        stupifyAudio = GameObject.Find("StupifyAudio").GetComponent<AudioSource>();
        AccioAudio = GameObject.Find("AccioAudio").GetComponent<AudioSource>();
        nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        //attempt on 2.26 to add "next level is 5" to fix troll scene
        if (nextLevel == 4 || nextLevel == 5)
        {
            responseCollector = GameObject.Find("ResponseCollector").GetComponent<ResponseCollector>();
        }
        wandSparks.SetActive(false);
        wandFlame.SetActive(false);
        rightHand = true; //delete 
        leftHand = false; //delete
        haloCount = GameObject.FindGameObjectsWithTag("haloObject").Length;
        noShot = true;

        //HALO PROTECTION LEVEL - breaks this...turn off halos at start, only to be activated on raycast collision
        //haloObjects = new List<GameObject>();
        haloObjects = GameObject.FindGameObjectsWithTag("haloObject");
        haloObjects.ToList();

        //to instantiate spell projectile
        stupifyProjectiles = new List<GameObject>();
        for (int i = 0; i < projectilesAmount; i++)
        {
            GameObject obj = Instantiate(stupifyProjectile);
            obj.SetActive(false);
            stupifyProjectiles.Add(obj);
        }
        //TRIED USING LIST TO GENERATE FIRST
        //for (int i = 0; i < haloCount; i++)
        //{
        //    haloObject = GameObject.FindGameObjectWithTag("haloObject");
        //    haloObjects.Add(haloObject);
        //}

        foreach (GameObject haloObject in haloObjects)
        {
            halo = haloObject.GetComponent("Halo");
            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
        }


        //trackedObj = GetComponent<SteamVR_TrackedObject>();
        laser = GetComponentInChildren<LineRenderer>();
        //if (rightHand)
        //{
        //    thisController = OVRInput.Controller.RTouch;


        //}
        //else
        //{
        //    thisController = OVRInput.Controller.LTouch;
        //}
    }

    // Update is called once per frame
    void Update() {

        //HAVING BIG ISSUE GETTING ACCIO SOUND EFFECT TO PLAY, wow.
        //if (acciotrue)
        //{
        //    AccioAudio.Play();
        //}
        //device = SteamVR_Controller.Input((int)trackedObj.index);
        //ADD: an if conditional? cant figure it

        //if spellsaid !null and has changed
        if (spellTimerTicking)
        {
            spellSaid = hpSpeechRecognitionEngine.spellSaid;
        }
        //spellSaid = hpSpeechRecognitionEngine.spellSaid;
        if (spellTimerTicking)
        {
            spellTime += Time.deltaTime;
            spellObjectResetTimer = true;
        }

        if (spellObjectResetTimer == true)
        {
            spellObjectReset += Time.deltaTime;
        }
        //return object to initial position for "replaying"
        if (spellObjectReset > 10)
        {
            hitObject.transform.position = hitObjectInitialPosition;
            hitObject.SetActive(true);
            spellObjectReset = 0;
            spellObjectResetTimer = false;
        }


        //controllerInputType = "Secondary";
        if (spellSaid == "Akio"|| OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            //start here: add^ if spellSaid == accio, then continue...clear spellword on GetUp

            wandSparks.SetActive(true);

            //NEED TO FIX ACCIO AUDIO
            acciotrue = true;

            spellTimerTicking = true;

            laser.gameObject.SetActive(true);

            //creates a laser 20 forward when pressed down & a hit point
            laser.SetPosition(0, gameObject.transform.position);
            laser.SetPosition(1, transform.TransformDirection(Vector3.forward) * 20);// NEW update this in all
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 20, laserMask))
            {
                hitObject = hit.transform.gameObject;
                hitObjectInitialPosition = hitObject.transform.position;
                Debug.Log("position is: " + hitObjectInitialPosition);



                //Play Audio on Crest Object or Troll Object
                //TRY if (hit object has audio source){}
                //hitObject.GetComponent<AudioSource>().Play();
                //playSound = true;

                //record which object was picked up
                if (nextLevel == 5)
                {
                    Debug.Log("next level is: " + nextLevel);
                    responseCollector.objectUsed = hitObject.ToString();
                }


                halo = hitObject.GetComponent("Halo");
                halo.GetType().GetProperty("enabled").SetValue(halo, true, null); //unsure what null does...
                hitObject.transform.position = Vector3.MoveTowards(hitObject.transform.position, oculusPlayer.transform.position * wandGrabPosition, moveSpeed);
                hitObject.GetComponent<Rigidbody>().isKinematic = true;
                hitObject.GetComponent<Rigidbody>().useGravity = false;
            }
            else //THIS MIGHT CAUSE SLOW DOWN!!! "Get component in update," but hopefully its activated rarely
            {
                foreach (GameObject haloObject in haloObjects)
                {
                    halo = haloObject.GetComponent("Halo");
                    halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                }
            }

            if (spellTime > spellTimeEnd)
            {
                //might need to put in ^ halo script here to deactivate halo again...but not sure
                //playSound = false;
                wandSparks.SetActive(false);
                hitObject.SetActive(false);
                spellTimerTicking = false;
                spellTime = 0;
                laser.gameObject.SetActive(false);
                //teleportAimerObject.SetActive(false);
                //hitObject.GetComponent<Rigidbody>().isKinematic = true; // do we want this false?
                //hitObject.GetComponent<Rigidbody>().useGravity = true;
                //hitObject.transform.parent = null;
                spellSaid = null;
                //add line item here to cancel BUTTON PRESS?

                if (responseCollector.objectUsed != null)
                {
                    typeWriterEffect.fullTextCount = 6;
                    typeWriterEffect.ShowText();
                    SceneManager.LoadScene(nextLevel);
                }
            }
        }

        //for turning off accio spell on release
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
        {
            wandSparks.SetActive(false);
            hitObject.SetActive(false);
            spellTimerTicking = false;
            spellTime = 0;
            laser.gameObject.SetActive(false);
            spellSaid = null;
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            wandFlame.SetActive(false);
            hitObject.SetActive(false);
            spellTimerTicking = false;
            spellTime = 0;
            laser.gameObject.SetActive(false);
            spellSaid = null;
        }

        if (spellSaid == "Wingardium Leviosa")
        {
            wandSparks.SetActive(true);
            spellTimerTicking = true;
            laser.gameObject.SetActive(true);
            //teleportAimerObject.SetActive(true);

            laser.SetPosition(0, gameObject.transform.position);
            laser.SetPosition(1, transform.TransformDirection(Vector3.forward) * 20);// NEW update this in all
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 20, laserMask))
            {
                halo = hitObject.GetComponent("Halo");
                halo.GetType().GetProperty("enabled").SetValue(halo, true, null); //unsure what null does...
                hitObject = hit.transform.gameObject;
                hitObject.GetComponent<Rigidbody>().isKinematic = true;
                hitObject.GetComponent<Rigidbody>().useGravity = false;
                hitObject.transform.parent = wandHand.transform;
            }
            else //THIS MIGHT CAUSE SLOW DOWN!!! "Get component in update," but hopefully its activated rarely
            {
                foreach (GameObject haloObject in haloObjects)
                {
                    halo = haloObject.GetComponent("Halo");
                    halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                }
            }
            if (spellTime > spellTimeEnd)
            {
                wandSparks.SetActive(false);
                spellTimerTicking = false;
                spellTime = 0;
                laser.gameObject.SetActive(false);
                //teleportAimerObject.SetActive(false);
                hitObject.GetComponent<Rigidbody>().isKinematic = true;
                hitObject.GetComponent<Rigidbody>().useGravity = true;
                hitObject.transform.parent = null;
                spellSaid = null;
            }
        }

        if (spellSaid == "Stupify" && noShot)
        {
            wandSparks.SetActive(true);
            stupifyAudio.Play();
            spellTimerTicking = true;
            currentProjectile = stupifyProjectiles[currentProjectileNumber];
            currentProjectile.SetActive(true);
            currentProjectile.transform.position = wandHand.transform.position;
            currentProjectile.transform.rotation = wandHand.transform.rotation;
            //currentProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileForce);
            currentProjectileNumber++;
            noShot = false;
            //spellSaid = null; //doesnt turn off quickly enough???
        
        if (noShot == false)
        {
            currentProjectile.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * projectileForce);
        }
        if (spellTime > spellTimeEnd)
        {
            noShot = true;
            currentProjectile.SetActive(false);
            spellTime = 0;
            spellTimerTicking = false;
            spellSaid = null;
        }

    }
        if (spellSaid == "Incendio" || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            wandFlame.SetActive(true);
            //flameAudio.Play();
            spellTimerTicking = true;
            laser.gameObject.SetActive(true);
            //teleportAimerObject.SetActive(true);

            laser.SetPosition(0, gameObject.transform.position);
            laser.SetPosition(1, transform.TransformDirection(Vector3.forward) * 100);// NEW update this in all
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 100, laserMask))
            {            
                hitObject = hit.transform.gameObject;
                hitObjectInitialPosition = hitObject.transform.position;
                Debug.Log("position is: " + hitObjectInitialPosition);

                if (hitObject.name == "Troll")
                {
                    destroyProjectile.trollDeath();
                }
            }
            if (spellTime > spellTimeEnd)
            {
                hitObject.SetActive(false);
                wandFlame.SetActive(false);
                spellTime = 0;
                spellTimerTicking = false;
                spellSaid = null;
                laser.gameObject.SetActive(false);
                //Add section here to cancel BUTTON PRESS?
            }

        }
        //else if (leftHand)
        //{
        //}
    }
}
