using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace BNG
{
    public class WandSpellsManager : GrabbableEvents
    {
        //public SteamVR_TrackedObject trackedObj;
        //public SteamVR_Controller.Device device;

        //Leviosa & Accio
        private LineRenderer laser;
        //public GameObject teleportAimerObject;
        public LayerMask laserMask; //applies to objects so that only they are interactable (layermasks)
                                    //private OVRInput.Controller thisController;
        public GameObject wandSparks;
        public GameObject wandFlame;
        public GameObject hitObject;

        public float moveSpeed = 0.001f; //how fast the accio'd object moves to the players wand
        private Ray ray;

        public HPSpeechRecognitionEngine hpSpeechRecognitionEngine;//TODO - remove
        public string spellSaid;//TODO - remove
        public List<GameObject> stupifyProjectiles;
        public GameObject stupifyProjectile;
        public int projectilesAmount;
        public int currentProjectileNumber;
        private Rigidbody projectileRigidbody;
        public GameObject currentProjectile;
        public float projectileForce;
        private bool noShot;
        public ResponseCollector responseCollector;
        public int nextLevel;//TODO - remove

        public bool playSound = false;
        AudioSource stupifyAudio;
        AudioSource flameAudio;
        AudioSource AccioAudio;
        bool acciotrue = false;
        public DestroyProjectile destroyProjectile;
        private Vector3 hitObjectInitialPosition;

        // Use this for initialization
        void Start()
        {
            stupifyAudio = GameObject.Find("StupifyAudio").GetComponent<AudioSource>();
            AccioAudio = GameObject.Find("AccioAudio").GetComponent<AudioSource>();

            wandSparks.SetActive(false);
            wandFlame.SetActive(false);

            noShot = true;

            //to instantiate spell projectile
            stupifyProjectiles = new List<GameObject>();
            for (int i = 0; i < projectilesAmount; i++)
            {
                GameObject obj = Instantiate(stupifyProjectile);
                obj.SetActive(false);
                stupifyProjectiles.Add(obj);
            }

            laser = GetComponentInChildren<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {


        }

        public override void OnTrigger(float triggerValue)
        {
            DetermineSpell();

            ActivateSpell();

            StartCoroutine(PlaySelectedSpell());

            base.OnTrigger(triggerValue);
        }

        public override void OnTriggerUp()
        {
            DeactivateSpell();
        }

        private void DetermineSpell()
        {
            //TODO
            spellSaid = "Accio";//vfx
        }

        private void ActivateSpell()
        {
            wandSparks.SetActive(true);

            //Play Audio on Crest Object or Troll Object
            //TRY if (hit object has audio source){}
            //hitObject.GetComponent<AudioSource>().Play();
            //playSound = true;

            //NEED TO FIX ACCIO AUDIO
            acciotrue = true;

            laser.gameObject.SetActive(true);
        }

        private IEnumerator PlaySelectedSpell()
        {
            if (spellSaid == "Accio")
            {
                while (true)
                {
                    //creates a laser 20 forward when pressed down & a hit point
                    laser.SetPosition(0, gameObject.transform.position);
                    laser.SetPosition(1, transform.TransformDirection(Vector3.forward) * 20);// NEW update this in all
                    RaycastHit hit;

                    if (Physics.Raycast(transform.position, transform.forward, out hit, 20, laserMask))
                    {
                        hitObject = hit.transform.gameObject;

                        if (hitObjectInitialPosition == new Vector3(0, 0, 0))
                        {
                            hitObjectInitialPosition = hitObject.transform.position;
                            Debug.Log("position is: " + hitObjectInitialPosition);

                            //TDOO - need?
                            hitObject.GetComponent<Rigidbody>().isKinematic = true;
                            hitObject.GetComponent<Rigidbody>().useGravity = false;
                        }

                        hitObject.transform.position = Vector3.MoveTowards(hitObject.transform.position, gameObject.transform.position + new Vector3(0,0,1), moveSpeed);
                    }

                    yield return null;
                }
            }
        }

        private void DeactivateSpell()
        {
            //might need to put in ^ halo script here to deactivate halo again...but not sure
            //playSound = false;
            wandSparks.SetActive(false);
            hitObject.SetActive(false);

            laser.gameObject.SetActive(false);
            //teleportAimerObject.SetActive(false);
            hitObject.GetComponent<Rigidbody>().isKinematic = true; // do we want this false?
            hitObject.GetComponent<Rigidbody>().useGravity = true;
            //hitObject.transform.parent = null;
            spellSaid = null;
            //add line item here to cancel BUTTON PRESS?

            hitObject.transform.position = hitObjectInitialPosition;

            //reset
            hitObjectInitialPosition = new Vector3(0, 0, 0);

            hitObject.SetActive(true);
            hitObject = null;

            StopCoroutine(PlaySelectedSpell());
        }
    }
}

