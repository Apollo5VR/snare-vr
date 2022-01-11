using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRayCast : MonoBehaviour
{
    /// <summary>
    /// How far we can shoot in meters
    /// </summary>
    public float MaxRange = 25f;

    /// <summary>
    /// Where our raycast or projectile will spawn from
    /// </summary>
    [Tooltip("Where our raycast or projectile will start from.")]
    public Transform MuzzlePointTransform;

    [Header("Raycast Options : ")]
    public int validLayer = 8;

    public float firingRate = 0.2f;
    private float lastShotTime;

    public GameObject HitFXPrefab; //{ get; private set; }

    public GameObject flameCollider;
    //Script Purpose: To create a countdown clock that respawns zombies

    public GameObject zombieAgent;
    public float originalTime;
    public float timeCounter; //TODO - private
    public GameObject[] randomSpawnLocations;
    public int randomNumber; //TODO - private

    public bool start = false;

    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        //randomSpawnLocations = GameObject.FindGameObjectsWithTag("SpawnLocation");
        //timeCounter = timeDifficulty;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            //if the count down reaches 0, respawn another zombie
            if (timeCounter <= 0)
            {

                //select one of the random spawn locations
                randomNumber = Random.Range(0, 3);
                Debug.Log("The exact random location is: " + randomNumber);

                //spawn at chosen random location
                GameObject wolf = Instantiate(zombieAgent, randomSpawnLocations[randomNumber].transform.position, new Quaternion(0, -180, 0, 0));
                wolf.SetActive(true);

                //reset counter to the original time
                timeCounter = originalTime;
            }

            //start countdown
            timeCounter = timeCounter - Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == validLayer)
        {
            //TODO - add a outline highlight so player can still see enemy

            ApplyParticleFX(other.transform.position, Quaternion.identity);

            StartCoroutine(DestroyAfterDelay(other.gameObject));

            score++;
        }

        //depr - not needed right now
        // push object if rigidbody
        /*
        Rigidbody hitRigid = other.attachedRigidbody;
        if (hitRigid != null)
        {
            hitRigid.AddForceAtPosition(BulletImpactForce * MuzzlePointTransform.forward, hit.point);
        }
        */

        // Damage if possible
        /*
        Damageable d = hit.collider.GetComponent<Damageable>();
        if (d)
        {
            d.DealDamage(Damage, hit.point, hit.normal, true, gameObject, hit.collider.gameObject);

            if (onDealtDamageEvent != null)
            {
                onDealtDamageEvent.Invoke(Damage);
            }
        }
        

        // Call event
        if (onRaycastHitEvent != null)
        {
            onRaycastHitEvent.Invoke(hit);
        }
        */
    }

    private IEnumerator DestroyAfterDelay(GameObject hitObj)
    {
        yield return new WaitForSeconds(2);

        RePoolEnemy(hitObj);
    }

    private void RePoolEnemy(GameObject objForRepool)
    {
        objForRepool.SetActive(false);
        objForRepool.transform.position = new Vector3(0, 0, 0);
    }

    public virtual void ApplyParticleFX(Vector3 position, Quaternion rotation)
    {
        if (HitFXPrefab)
        {
            GameObject impact = Instantiate(HitFXPrefab, position, rotation) as GameObject;

            // Attach bullet hole to object if possible
            /*
            BulletHole hole = impact.GetComponent<BulletHole>();
            if (hole)
            {
                hole.TryAttachTo(attachTo);
            }
            */
        }
    }
}
