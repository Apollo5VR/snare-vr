using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
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
    public int deathCount;
    public int attackSuccessCount;
    public AudioClip wolfSpawn;
    public AudioClip wolfAttack;
    public AudioClip wolfDeath;

    private AudioSource wolfAudio;

    private void Start()
    {
        wolfAudio = GetComponent<AudioSource>();

        ScriptsConnector.Instance.OnStartWolfSequence += StartWolfSequence;
        ScriptsConnector.Instance.OnWolfDeath += DestroyAfterDelay;
    }

    public void StartWolfSequence()
    {
        StartCoroutine(WolfSequence());
    }

    // Update is called once per frame
    private IEnumerator WolfSequence()
    {
        while ((deathCount + attackSuccessCount) < 6)
        {
            float waitTime = 1;

            //if the count down reaches 0, respawn another zombie
            if (timeCounter <= 0)
            {
                //play audio
                wolfAudio.PlayOneShot(wolfSpawn);

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
            timeCounter = timeCounter - waitTime;

            yield return new WaitForSeconds(waitTime);
        }

        Debug.Log("done");
    }

    private void DestroyAfterDelay(GameObject hitObj, bool isKilled)
    {
        if(isKilled)
        {
            wolfAudio.PlayOneShot(wolfDeath);
            deathCount++;
        }
        else
        {
            wolfAudio.PlayOneShot(wolfAttack);
            attackSuccessCount++;
        }

        if(attackSuccessCount == 6)
        {
            ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "UH OH THEY DESTROYED YOUR TRAP. SET ANOTHER.");
        }
        else if ((deathCount + attackSuccessCount) == 6)
        {
            ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "ALL THE WOLVES ARE DEAD! (OR FULL)");
        }

        //RePoolEnemy(hitObj);
    }

    //TODO - actually create an object pool
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

    private void OnDestroy()
    {
        if(ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnStartWolfSequence -= StartWolfSequence;
            ScriptsConnector.Instance.OnWolfDeath -= DestroyAfterDelay;
        }
    }
}
