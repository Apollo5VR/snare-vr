using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

namespace BNG
{
    public class WandSpellsController : GrabbableEvents
    {
        public SpellSelectionWheelManager spellSelectionWheelManager;
        public LayerMask spellLaserMask; //applies to objects so that only they are interactable (layermasks)
        public LayerMask spellWheelLaserMask;
        public GrabbableHaptics wandHaptics;

        public GameObject player;
        public GameObject wandSparks;
        public GameObject wandFirePrefab;
        public GameObject wandFlashPrefab;

        public ResponseCollector responseCollector;

        public AudioSource spellAudio;

        public DigitalRuby.LightningBolt.LightningBoltScript lightingBoltScript;

        //test stuff
        //public GameObject debugHitObject;
            //public bool debugTrue = false;

        public CommonEnums.AvailableSpells spellSelected;

        private GameObject wandFire;
        private GameObject hitObject;
        private bool spellActive = false;
        private int activeScene = 0;

        private int closeCount = 0;
        private bool isStart = true;
        private float moveSpeed = 0.05f; //how fast the accio'd object moves to the players wand
        private int nextLevel;

        private Ray ray;
        private LineRenderer laser;

        private Vector3 hitObjectInitialPosition;

        #region deactives
        /*
        //TODO set to private?
        public GameObject[] haloObjects;
        private Component halo;
        public int haloCount;
        private GameObject haloObject;
        */
        #endregion

        // Use this for initialization
        void Start()
        {
            wandSparks.SetActive(false);
            wandFire = Instantiate(wandFirePrefab);
            wandFire.SetActive(false);

            //depreciated - stupify 
            //wandFlash = Instantiate(wandFlashPrefab, gameObject.transform);
            //wandFlash.SetActive(false);

            laser = GetComponentInChildren<LineRenderer>();
            laser.gameObject.SetActive(false);
            spellSelectionWheelManager.gameObject.SetActive(false);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //TODO - refactor - remove the necessity for this - scene management should only be checked in ProgressionController
            activeScene = SceneManager.GetActiveScene().buildIndex;
            if (activeScene == 4)
            {
                spellSelected = CommonEnums.AvailableSpells.None;
            }
        }

        private void ActivateSpellWheel()
        {
            //spellSelectionWheelManager.gameObject.transform.position = player.transform.position + new Vector3(0, 0f,0.25f);
            //spellSelectionWheelManager.gameObject.transform.rotation = new Quaternion(spellSelectionWheelManager.gameObject.transform.rotation.x, player.transform.position.y, spellSelectionWheelManager.gameObject.transform.rotation.z, spellSelectionWheelManager.gameObject.transform.rotation.w);
            spellSelectionWheelManager.gameObject.SetActive(true);
        }

        public override void OnGrab(Grabber grabber)
        {
            if (isStart)
            {
                Debug.Log("Wand Grabbed");
                isStart = false;
                ProgressionController.Instance.OnLoadNextScene(1);

                base.OnGrab(grabber);
            }
        }

        public override void OnButton1Up()
        {
            if(!isStart)
            {
                ActivateSpellWheel();
            }

            base.OnButton1Up();
        }

        //close helpers
        //TODO - if crashes - look here
        public override void OnButton2()
        {
            Debug.Log("button Y held");

            closeCount++;

            if (spellActive && closeCount > 1000)
            {
                Application.Quit();
            }

            base.OnButton2();
        }

        public override void OnButton2Up()
        {
            closeCount = 0;
        }

        public override void OnTrigger(float triggerValue)
        {
            wandHaptics.doHaptics(wandHaptics.currentGrabber.HandSide);

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

            base.OnTrigger(triggerValue);
        }

        private void DetermineAndToggleSpell(bool spellActive)
        {
            if (spellActive)
            {
                wandSparks.SetActive(true);
                laser.gameObject.SetActive(true);
                spellAudio.Play();

                switch (spellSelected)
                {
                    case CommonEnums.AvailableSpells.Accio:
                        StartCoroutine(CastAccio());
                        break;
                    case CommonEnums.AvailableSpells.Incendio:
                        StartCoroutine(CastIncendio());
                        break;
                    case CommonEnums.AvailableSpells.WingardiumLeviosa:
                        StartCoroutine(CastWingardiumLeviosa());
                        break;
                    case CommonEnums.AvailableSpells.Stupify:
                        StartCoroutine(CastLightning());
                        break;
                    case CommonEnums.AvailableSpells.None:
                        //TODO - refactor this - dont like the conditional necessity for this
                        if (activeScene != 4) // "SortingHatQuestioningScene"
                        {
                            StartCoroutine(CastSputter());
                        }
                        else
                        {
                            StartCoroutine(CastChallengeSelectionRaycast());
                        }
                        
                        break;
                }
            }
            else
            {
                wandSparks.SetActive(false);
                laser.gameObject.SetActive(false);
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

                ray = new Ray(transform.position, transform.forward);  /*faster moethod: https://answers.unity.com/questions/949222/is-raycast-efficient-in-update.html */
                if (Physics.Raycast(ray, out hit, 20, spellLaserMask))
                {
                    if (hitObject != hit.transform.gameObject)
                    {
                        hitObject = hit.transform.gameObject;
                        hitObject.GetComponent<Rigidbody>().isKinematic = true;
                        hitObject.GetComponent<Rigidbody>().useGravity = false;
                    }

                    hitObject.transform.position = Vector3.MoveTowards(hitObject.transform.position, gameObject.transform.position + new Vector3(0,0,2.5f), moveSpeed);
                }

                yield return null;
            }

            DeactivateAccio();
        }

        private void DeactivateAccio()
        {
            //might need to put in ^ halo script here to deactivate halo again...but not sure
            //playSound = false;
            if (hitObject != null)
            {
                hitObject.GetComponent<Rigidbody>().isKinematic = false;
                hitObject.GetComponent<Rigidbody>().useGravity = true;
                hitObject = null;
            }
        }

        private IEnumerator CastIncendio()
        {
            while (spellActive)
            {
                //creates a laser 20 forward when pressed down & a hit point
                laser.SetPosition(0, gameObject.transform.position);
                laser.SetPosition(1, transform.TransformDirection(Vector3.forward) * 20);// NEW update this in all
                RaycastHit hit;

                ray = new Ray(transform.position, transform.forward);  /*faster moethod: https://answers.unity.com/questions/949222/is-raycast-efficient-in-update.html */
                if (Physics.Raycast(ray, out hit, 20, spellLaserMask))
                {
                    if (hitObject != hit.transform.gameObject)
                    {
                        hitObject = hit.transform.gameObject;
                        wandFire.transform.position = hitObject.transform.position;
                        wandFire.SetActive(true);
                    }

                }

                yield return null;
            }

            DeactivateIncendio();
        }

        private void DeactivateIncendio()
        {
            //might need to put in ^ halo script here to deactivate halo again...but not sure
            //playSound = false;
            if (hitObject != null)
            {
                hitObject = null;
                //wandFire.SetActive(false);
            }
        }

        private IEnumerator CastLightning() //previously Stupify
        {
            while (spellActive)
            {
                //creates a laser 20 forward when pressed down & a hit point
                laser.SetPosition(0, gameObject.transform.position);
                laser.SetPosition(1, transform.TransformDirection(Vector3.forward) * 50);// NEW update this in all
                RaycastHit hit;

                ray = new Ray(transform.position, transform.forward);  /*faster moethod: https://answers.unity.com/questions/949222/is-raycast-efficient-in-update.html */
                if (Physics.Raycast(ray, out hit, 50, spellLaserMask))
                {
                    if (hitObject != hit.transform.gameObject)
                    {
                        hitObject = hit.transform.gameObject;
                        lightingBoltScript.EndObject = hitObject;

                        yield return null;

                        lightingBoltScript.Trigger();
                        //lightingBoltScript.gameObject.SetActive(true);
                        //this.GetComponent<LineRenderer>().enabled = true;

                        //depreciated (stupify)
                        //wandFlash.transform.position = hitObject.transform.position;
                        //yield return null;
                        //wandFlash.SetActive(true);
                    }

                    lightingBoltScript.Trigger();
                }

                yield return null;
            }

            //Scene 3 Proving - Troll
            if(hitObject != null && hitObject.name == "TheTroll")
            {
                if (ResponseCollector.Instance.OnCheckAcceptableTags.Invoke(hitObject?.tag) == CommonEnums.HouseResponses.Gryfindor)
                {
                    if(TrollController.Instance != null)
                    {
                        TrollController.Instance.OnTrollSceneResponseSelected?.Invoke(CommonEnums.HouseResponses.Gryfindor);
                    }
                }
            }

            DeactivateLightning();
        }

        private void DeactivateLightning() //previously stupify
        {
            lightingBoltScript.EndObject = null;
            //lightingBoltScript.gameObject.SetActive(false);

            //this.GetComponent<LineRenderer>().enabled = false;
            //might need to put in ^ halo script here to deactivate halo again...but not sure
            //playSound = false;
            if (hitObject != null)
            {
                hitObject = null;

                //depreciated - stupify
                //wandFlash.SetActive(false);
            }
        }

        //TODO - consider turning this on indefinitely for object once activated 1x
        private IEnumerator CastWingardiumLeviosa()
        {
            float minimum = 0;
            float maximum = 0;

            // starting value for the Lerp
            float t = 0.0f;

            while (spellActive)
            {
                //creates a laser 20 forward when pressed down & a hit point
                laser.SetPosition(0, gameObject.transform.position);
                laser.SetPosition(1, transform.TransformDirection(Vector3.forward) * 20);// NEW update this in all
                RaycastHit hit;

                ray = new Ray(transform.position, transform.forward);  /*faster moethod: https://answers.unity.com/questions/949222/is-raycast-efficient-in-update.html */
                if (Physics.Raycast(ray, out hit, 20, spellLaserMask))
                {
                    if (hitObject != hit.transform.gameObject)
                    {
                        minimum = hit.transform.position.y + 0.5f;
                        maximum = hit.transform.position.y + 1.0f;

                        // starting value for the Lerp
                        t = 0.0f;

                        hitObject = hit.transform.gameObject;

                        //depreciated (testing only)
                        //StartCoroutine(FloatObject(hit.transform.gameObject, hit.transform.position));

                        //depreciated 
                        /*
                        hitObject.GetComponent<Rigidbody>().isKinematic = true;
                        hitObject.GetComponent<Rigidbody>().useGravity = false;
                        hitObject.transform.parent = gameObject.transform;
                        */
                    }
                }

                // animate the position of the game object...
                if(hitObject != null)
                {
                    hitObject.transform.position = new Vector3(hitObject.transform.position.x, Mathf.Lerp(minimum, maximum, t), hitObject.transform.position.z);
                }            

                // .. and increase the t interpolater
                t += 0.75f * Time.deltaTime;

                // now check if the interpolator has reached 1.0
                // and swap maximum and minimum so game object moves
                // in the opposite direction.
                if (t > 1.0f)
                {
                    float temp = maximum;
                    maximum = minimum;
                    minimum = temp;
                    t = 0.0f;
                }

                yield return null;
            }

            //note: object has gravity enabled, so should just fall to ground once lerp stops
            //Scene 3 Proving - Statue
            if (hitObject != null && hitObject.name == "TheKnight")
            {
                if (ResponseCollector.Instance.OnCheckAcceptableTags.Invoke(hitObject.tag) == CommonEnums.HouseResponses.Ravenclaw)
                {
                    if (TrollController.Instance != null)
                    {
                        TrollController.Instance.OnTrollSceneResponseSelected?.Invoke(CommonEnums.HouseResponses.Ravenclaw);
                    }
                }
            }

            DeactivateWingardiumLeviosa();
        }

        /*
        public void Update()
        {
            if (debugTrue)
            {
                debugTrue = false;
                StartCoroutine(FloatObject(debugHitObject, debugHitObject.transform.position));
            }
        }
        */

        //depreciated - testing only
        private IEnumerator FloatObject(GameObject floatObject, Vector3 startingHeight)
        {
            float minimum = startingHeight.y + 0.5f;
            float maximum = startingHeight.y + 1.0f;

            // starting value for the Lerp
            float t = 0.0f;

            yield return null;

            while (true) //spellActive
            {
                // animate the position of the game object...
                floatObject.transform.position = new Vector3(floatObject.transform.position.x, Mathf.Lerp(minimum, maximum, t), floatObject.transform.position.z);

                // .. and increase the t interpolater
                t += 0.75f * Time.deltaTime;

                yield return null;

                // now check if the interpolator has reached 1.0
                // and swap maximum and minimum so game object moves
                // in the opposite direction.
                if (t > 1.0f)
                {
                    float temp = maximum;
                    maximum = minimum;
                    minimum = temp;
                    t = 0.0f;
                }
            }
        }

        private void DeactivateWingardiumLeviosa()
        {
            //might need to put in ^ halo script here to deactivate halo again...but not sure
            //playSound = false;
            if (hitObject != null)
            {
                hitObject.transform.parent = null;
                hitObject.GetComponent<Rigidbody>().isKinematic = false;
                hitObject.GetComponent<Rigidbody>().useGravity = true;
                hitObject = null;
            }
        }


        private IEnumerator CastSputter()
        {
            while (spellActive)
            {
                wandSparks.SetActive(true);

                yield return new WaitForSeconds(0.5f);

                wandSparks.SetActive(false);

                yield return new WaitForSeconds(0.25f);
            }

            DeactivateSputter();
        }

        private void DeactivateSputter()
        {
            //TODO 
        }

        private IEnumerator CastChallengeSelectionRaycast()
        {
            while (spellActive)
            {
                //creates a laser 20 forward when pressed down & a hit point
                laser.SetPosition(0, gameObject.transform.position);
                laser.SetPosition(1, transform.TransformDirection(Vector3.forward) * 50);// NEW update this in all
                RaycastHit hit;

                ray = new Ray(transform.position, transform.forward);  /*faster moethod: https://answers.unity.com/questions/949222/is-raycast-efficient-in-update.html */
                if (Physics.Raycast(ray, out hit, 50, spellLaserMask))
                {
                    if (hitObject != hit.transform.gameObject)
                    {
                        hitObject = hit.transform.gameObject;
                        CommonEnums.HouseResponses response = ResponseCollector.Instance.OnCheckAcceptableTags.Invoke(hitObject.tag);
                        ResponseCollector.Instance.OnToggleSceneSelectionResponse?.Invoke();
                        ResponseCollector.Instance.OnResponseSelected?.Invoke(response);
                    }
                }

                yield return null;
            }

            DeactivateChallengeSelectionRaycast();
        }

        private void DeactivateChallengeSelectionRaycast()
        {
            hitObject = null;
        }

        //TODO - not quite ready (secondary item)
        /*
        private IEnumerator ToggleHitHalo(GameObject hitObject)
        {
            halo = hitObject.GetComponent("Halo");
            halo.GetType().GetProperty("enabled").SetValue(halo, true, null); //unsure what null does...

            yield return new WaitForSeconds(2f);


            halo.GetType().GetProperty("enabled").SetValue(halo, false, null); //unsure what null does...
        }
        */

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}

