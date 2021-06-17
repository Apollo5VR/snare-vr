using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace BNG
{
    public class WandSpellsController : GrabbableEvents
    {
        public SpellSelectionWheelManager spellSelectionWheelManager;
        public LayerMask spellLaserMask; //applies to objects so that only they are interactable (layermasks)
        public LayerMask spellWheelLaserMask;

        public GameObject player;
        public GameObject wandSparks;
        public GameObject wandFirePrefab;
        public GameObject wandFlashPrefab;

        public float moveSpeed = 0.001f; //how fast the accio'd object moves to the players wand
        public ResponseCollector responseCollector;
        public int nextLevel;//TODO - remove

        public AudioSource spellAudio;
        public Vector3 hitObjectInitialPosition;

        public CommonEnums.availableSpells spellSelected;
        private Ray ray;
        private LineRenderer laser;

        private GameObject wandFire;
        private GameObject wandFlash;
        private GameObject hitObject;
        private bool spellActive = false;

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
            wandFlash = Instantiate(wandFlashPrefab);
            wandFlash.SetActive(false);

            laser = GetComponentInChildren<LineRenderer>();
            laser.gameObject.SetActive(false);
            spellSelectionWheelManager.gameObject.SetActive(false);
        }

        //public void Update()
        //{
        //    if (!spellActive)
        //    {
        //        //creates a laser 20 forward when pressed down & a hit point
        //        laser.SetPosition(0, gameObject.transform.position);
        //        laser.SetPosition(1, transform.TransformDirection(Vector3.forward) * 20);// NEW update this in all
        //        RaycastHit hit;

        //        ray = new Ray(transform.position, transform.forward);  /*faster moethod: https://answers.unity.com/questions/949222/is-raycast-efficient-in-update.html */
        //        if (Physics.Raycast(ray, out hit, 20, spellLaserMask))
        //        {
        //            if (hitObject != hit.transform.gameObject)
        //            {
        //                hitObject = hit.transform.gameObject;
        //                StartCoroutine(ToggleHitHalo(hitObject));
        //            }
        //        }
        //    }
        //}

        private void ActivateSpellWheel()
        {
            //spellSelectionWheelManager.gameObject.transform.position = player.transform.position + new Vector3(0, 0f,0.25f);
            //spellSelectionWheelManager.gameObject.transform.rotation = new Quaternion(spellSelectionWheelManager.gameObject.transform.rotation.x, player.transform.position.y, spellSelectionWheelManager.gameObject.transform.rotation.z, spellSelectionWheelManager.gameObject.transform.rotation.w);
            spellSelectionWheelManager.gameObject.SetActive(true);
        }

        public override void OnButton1Up()
        {

            ActivateSpellWheel();

            base.OnButton1Up();
        }

        public override void OnTrigger(float triggerValue)
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
                    case CommonEnums.availableSpells.Accio:
                        StartCoroutine(CastAccio());
                        break;
                    case CommonEnums.availableSpells.Incendio:
                        StartCoroutine(CastIncendio());
                        break;
                    case CommonEnums.availableSpells.WingardiumLeviosa:
                        StartCoroutine(CastWingardiumLeviosa());
                        break;
                    case CommonEnums.availableSpells.Stupify:
                        StartCoroutine(CastStupify());
                        break;
                    case CommonEnums.availableSpells.None:
                        StartCoroutine(CastSputter());
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
                wandFire.SetActive(false);
            }
        }

        private IEnumerator CastStupify()
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
                        wandFlash.transform.position = hitObject.transform.position;
                        yield return null;
                        wandFlash.SetActive(true);
                    }

                }

                yield return null;
            }

            DeactivateStupify();
        }

        private void DeactivateStupify()
        {
            //might need to put in ^ halo script here to deactivate halo again...but not sure
            //playSound = false;
            if (hitObject != null)
            {
                hitObject = null;
                wandFlash.SetActive(false);
            }
        }

        private IEnumerator CastWingardiumLeviosa()
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
                        hitObject.transform.parent = gameObject.transform;
                    }
                }

                yield return null;
            }

            DeactivateWingardiumLeviosa();
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
    }
}

