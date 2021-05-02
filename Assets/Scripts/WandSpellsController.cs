using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace BNG
{
    public class WandSpellsController : GrabbableEvents
    {
        //public SteamVR_TrackedObject trackedObj;
        //public SteamVR_Controller.Device device;

        //Leviosa & Accio
        //public GameObject teleportAimerObject;
        public LayerMask spellLaserMask; //applies to objects so that only they are interactable (layermasks)
        public LayerMask spellWheelLaserMask;

        public GameObject wandSparks;
        public GameObject wandFlame;
        public GameObject hitObject;

        public float moveSpeed = 0.001f; //how fast the accio'd object moves to the players wand

        public HPSpeechRecognitionEngine hpSpeechRecognitionEngine;//TODO - remove
        public List<GameObject> stupifyProjectiles;
        public GameObject stupifyProjectile;
        public int projectilesAmount;
        public int currentProjectileNumber;
        public GameObject currentProjectile;
        public float projectileForce;
        public ResponseCollector responseCollector;
        public int nextLevel;//TODO - remove

        public bool playSound = false;
        AudioSource stupifyAudio;
        AudioSource flameAudio;
        AudioSource AccioAudio;
        bool spellActive = false;
        public bool spellSelectionActive = false;
        public DestroyProjectile destroyProjectile;
        private Vector3 hitObjectInitialPosition;

        public availableSpells spellSelected;
        public enum availableSpells
        {
            None = -1,
            Accio = 0, //Distance grab 
            Stupify = 1, //Paralyze 
            WingardiumLeviosa = 2, //Float 
            Incendio = 3 //Burn
        }

        private Rigidbody projectileRigidbody;
        private bool noShot;
        private Ray ray;
        private LineRenderer laser;

        // Use this for initialization
        void Start()
        {


            //stupifyAudio = GameObject.Find("StupifyAudio").GetComponent<AudioSource>();
            //AccioAudio = GameObject.Find("AccioAudio").GetComponent<AudioSource>();

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

        //To do - Activate spell selection wheel

        private IEnumerator ActivateSpellWheel()
        {
            SpellSelectionWheelManager.Instance.gameObject.SetActive(true);

            while (spellSelectionActive)
            {
                //creates a laser 20 forward when pressed down & a hit point
                laser.SetPosition(0, gameObject.transform.position);
                laser.SetPosition(1, transform.TransformDirection(Vector3.forward) * 5);// NEW update this in all
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, 5, spellWheelLaserMask))
                {
                    hitObject = hit.transform.gameObject;

                    //TODO
                    if (hitObject == SpellSelectionWheelManager.Instance.spellButtons[0])
                    {
                        spellSelected = availableSpells.Accio;
                    }
                    else if (hitObject == SpellSelectionWheelManager.Instance.spellButtons[1])
                    {
                        spellSelected = availableSpells.Stupify;
                    }
                    else if (hitObject == SpellSelectionWheelManager.Instance.spellButtons[2])
                    {
                        spellSelected = availableSpells.WingardiumLeviosa;
                    }
                    else if (hitObject == SpellSelectionWheelManager.Instance.spellButtons[3])
                    {
                        spellSelected = availableSpells.Incendio;
                    }
                    else
                    {
                        spellSelected = availableSpells.None;
                    }

                    Debug.Log("hit wheel item: " + hitObject);
                }

                yield return null;
            }

            SpellSelectionWheelManager.Instance.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {


        }

        public override void OnButton1Up()
        {
            if (!spellSelectionActive)
            {
                StartCoroutine(ActivateSpellWheel());
                spellSelectionActive = true;
            }

            base.OnButton1Up();
        }

        public override void OnTriggerUp()
        {
            if (spellSelectionActive)
            {
                //To do - select / Change spell ?

                spellSelectionActive = false;
            }

            base.OnTriggerUp();
        }

        public override void OnTrigger(float triggerValue)
        {
            if (!spellSelectionActive)
            {
                if (triggerValue >= 0.75f)
                {
                    if (!spellActive)
                    {
                        spellActive = true;

                        DetermineAndToggleSpell(spellActive);
                    }
                }
                else
                {
                    if (spellActive)
                    {
                        spellActive = false;

                        DetermineAndToggleSpell(spellActive);
                    }
                }
            }

            base.OnTrigger(triggerValue);
        }

        private void DetermineAndToggleSpell(bool spellActive)
        {

            //Play Audio on Crest Object or Troll Object
            //TRY if (hit object has audio source){}
            //hitObject.GetComponent<AudioSource>().Play();
            //playSound = true;
            if (spellActive)
            {
                wandSparks.SetActive(true);
                laser.gameObject.SetActive(true);

                //TODO - remove this once selectionWheelSet
                //spellSelected = availableSpells.Accio;

                //TODO
                switch (spellSelected)
                {
                    case availableSpells.Accio:
                        StartCoroutine(CastAccio());
                        break;
                    case availableSpells.Incendio:

                        break;

                }
            }
            else
            {
                wandSparks.SetActive(false);
                laser.gameObject.SetActive(false);
                //spellSelected = availableSpells.None;

                ////TODO - remove - handled in cast
                //switch (spellSaid)
                //{
                //    case availableSpells.Accio:
                //        DeactivateAccio();
                //        break;
                //    case availableSpells.Incendio:

                //        break;

                //}
            }
        }

        private IEnumerator CastAccio()
        {
            while (spellActive)
            {
                //creates a laser 20 forward when pressed down & a hit point
                laser.SetPosition(0, gameObject.transform.position);
                laser.SetPosition(1, transform.TransformDirection(Vector3.forward) * 20);// NEW update this in all
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, 20, spellLaserMask))
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

            DeactivateAccio();
        }

        private void DeactivateAccio()
        {
            //might need to put in ^ halo script here to deactivate halo again...but not sure
            //playSound = false;
            if(hitObject != null)
            {
                hitObject.SetActive(false);
                hitObject.GetComponent<Rigidbody>().isKinematic = true; // do we want this false?
                hitObject.GetComponent<Rigidbody>().useGravity = true;
                hitObject.transform.position = hitObjectInitialPosition;
                hitObjectInitialPosition = new Vector3(0, 0, 0);
                hitObject.SetActive(true);
                hitObject = null;
            }
        }
    }
}

